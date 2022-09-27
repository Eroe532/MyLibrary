using EventStore.Client;

using EventStoreDBLibrary.Exeptions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventStoreDBLibrary.ESDB
{
    /// <summary>
    /// Класс для слушателя(подписчика на поток) в EventStoreDB Который записывает в бд информацию
    /// </summary>
    /// <typeparam name="TContext">Класс понтекста бд</typeparam>
    public abstract class ESDBSubscriber<TContext> where TContext : DbContext
    {
        /// <summary>
        /// Клиент EventStoreDB
        /// </summary>
        protected EventStoreClient _client;

        /// <summary>
        /// Фабрика контекстов Бд
        /// </summary>
        protected IDbContextFactory<TContext> _contextFactory;

        /// <summary>
        /// Имя потока
        /// </summary>
        protected readonly string _streamName;

        /// <summary>
        /// подписка начата или нет?
        /// </summary>
        protected bool IsSubscribed { get; private set; }

        /// <summary>
        /// Количество попыток переподписок на поток (-1 - не ограниченое кол-во переподписок)
        /// </summary>
        protected int _numbAttemps;

        /// <summary>
        /// логер
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Позиция ошибки
        /// </summary>
        protected ulong erorPosition = 0ul;

        /// <summary>
        /// Итерация ошибки
        /// </summary>
        protected int erorEteration = 0;

        /// <summary>
        /// подписчик
        /// </summary>
        protected StreamSubscription? _subscriber = null;


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="client">Клиент EventStoreDB</param>
        /// <param name="contextFactory">Фабрика контекстов Бд</param>
        /// <param name="logger">Логгер</param>
        /// <param name="streamName">Имя потока</param>
        /// <param name="numbAttemps">Количество попыток переподписок на поток (-1 - не ограниченое кол-во переподписок)</param>
        public ESDBSubscriber(EventStoreClient client, IDbContextFactory<TContext> contextFactory, ILogger logger, string streamName, int numbAttemps = -1)
        {
            _client = client;
            _contextFactory = contextFactory;
            _logger = logger;
            _streamName = streamName;
            _numbAttemps = numbAttemps;
        }

        /// <summary>
        /// ф-ия обработки получаемых Событиеов
        /// </summary>
        /// <param name="evnt">получаеммый из ESDb Событие</param>
        /// <returns></returns>
        protected abstract Task DefaultHandler(ResolvedEvent evnt);


        /// <summary>
        /// Функция запускаемая при старте подписки
        /// </summary>
        protected abstract Task SubscribeStartAction();

        /// <summary>
        /// обертка на проверку акульного состояния актульной модели данных на актульность
        /// (проверяет актуальность данных в posgre, при необходимости дочитывает события из стора)
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ChecActualStateAsync()
        {
            try
            {
                await SubscribeStartAction();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// метод подписки на получение Событиеов из EventStoreDB (по 1 потоку)
        /// </summary>
        /// <param name="position">позиция в потоке с которой начинаем смотреть (0ul - начало ulong.maxvalue - конец)</param>
        /// <param name="Handler"> Функция для обработки Событиеов</param>
        public async Task SubscribeStreamAsync(FromStream position, Func<ResolvedEvent, Task>? Handler = null)
        {
            _logger.LogInformation($"__Попытка_запустить_подписчика: {_streamName}");
            if (!await ChecActualStateAsync())
            {
                _logger.LogCritical($"Не удалось запустить подписчика: {_streamName}");
            }
            if (IsSubscribed == false)
            {
                _logger.LogInformation($"__Подписчик_({_streamName})_запущен");
                IsSubscribed = true;
                if (_numbAttemps >= 0)
                {
                    _numbAttemps -= 1;
                }
                _subscriber = await _client.SubscribeToStreamAsync(
                    _streamName,
                    position,
                    async (subscription, evnt, cancellationToken) =>
                    {
                        position = FromStream.After(evnt.OriginalEventNumber);
                        if (Handler == null)
                        {
                            await DefaultHandler(evnt);
                        }
                        else
                        {
                            await Handler(evnt);
                        }
                    },
                    resolveLinkTos: true,
                    subscriptionDropped: async (subscription, reason, exception) =>
                    {
                        if (_subscriber == null)
                        {
                            IsSubscribed = false;
                            _logger.LogError($"Подписчике {_streamName} отключен");
                        }
                        if (_numbAttemps == 0)
                        {
                            IsSubscribed = false;
                            _logger.LogCritical($"Подписчике {_streamName} количество достиг максимального количества перезапусков");
                        }
                        else
                        {
                            IsSubscribed = false;
                            _logger.LogError($"В подписчике {_streamName} Возникла ошибка, статус подписчика: {reason}");
                            await SubscribeStreamAsync(position);
                        }
                    }
                );

            }
        }

        /// <summary>
        /// Уничтожение подписчика при большом количестве одинаковых ошибок
        /// </summary>
        /// <param name="ex">Ошибка</param>
        /// <param name="position">позиция в потоке</param>
        /// <exception cref="Exception"></exception>
        protected void SubscriberDrop(Exception ex, ulong position)
        {
            if (erorPosition == position)
            {
                erorEteration++;
            }
            else
            {
                erorPosition = position;
                erorEteration = 1;
            }
            if (erorEteration > NumberAttemptsExceededException.Iterations)
            {
                var newExeption = new NumberAttemptsExceededException(ex, position, _streamName);
                _logger.LogError(newExeption, newExeption.Message);
                throw newExeption;
            }
        }
    }
}