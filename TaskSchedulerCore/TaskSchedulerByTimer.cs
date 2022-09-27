namespace TaskSchedulerCore
{
    /// <summary>
    /// Планировщик задач
    /// </summary>
    public partial class TaskSchedulerByTimer
    {
        /// <summary>
        /// Планировщик задач
        /// </summary>
        private static TaskSchedulerByTimer? _instance;

        /// <summary>
        /// Список таймеров
        /// </summary>
        private List<Timer> _timers = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        private TaskSchedulerByTimer() { }

        /// <summary>
        /// Доступ к планировщику задач
        /// </summary>
        public static TaskSchedulerByTimer Instance => _instance ??= new TaskSchedulerByTimer();

        /// <summary>
        /// Добавление задач
        /// </summary>
        public void ScheduleTask(params Timer[] timers)
        {
            _timers.AddRange(timers);
        }

        /// <summary>
        /// Добавление задачи
        /// </summary>
        /// <param name="hour">Время начала (часы)</param>
        /// <param name="min">Время начала (минуты)</param>
        /// <param name="interval">Интервал</param>
        /// <param name="timeFormat">Величина интервала</param>
        /// <param name="action">Действие</param>
        public void ScheduleTask((int hour, int min) time, double interval, TimeFormat timeFormat, Action action)
        {
            _timers.Add(TaskSchedulerExtension.GetTimer(time, interval, timeFormat, action));
        }

        /// <summary>
        /// Добавление задачи
        /// </summary>
        /// <param name="hour">Время начала (часы)</param>
        /// <param name="min">Время начала (минуты)</param>
        /// <param name="interval">Интервал</param>
        /// <param name="timeFormat">Величина интервала</param>
        /// <param name="task">Действие</param>
        public void ScheduleTask((int hour, int min) time, double interval, TimeFormat timeFormat, Func<Task> task)
        {
            _timers.Add(TaskSchedulerExtension.GetTimer(time, interval, timeFormat, task));
        }
    }
}