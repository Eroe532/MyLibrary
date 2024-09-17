namespace TaskSchedulerCore;

/// <summary>
/// Расширения и статичные функции
/// </summary>
public static class TaskSchedulerExtension
{
    #region Base
    /// <summary>
    /// Получение функции формирования интервала
    /// </summary>
    /// <param name="timeFormat">Величина интервала</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    internal static Func<double, TimeSpan> GetTimeFormat(this TimeFormat timeFormat)
    {
        return timeFormat switch
        {
            TimeFormat.FromDays => TimeSpan.FromDays,
            TimeFormat.FromHours => TimeSpan.FromHours,
            TimeFormat.FromMinutes => TimeSpan.FromMinutes,
            TimeFormat.FromSeconds => TimeSpan.FromSeconds,
            TimeFormat.FromMilliseconds => TimeSpan.FromMilliseconds,
            _ => throw new NotImplementedException()
        };
    }

    /// <summary>
    /// Получение таймера
    /// </summary>
    /// <param name="hour">Время начала (часы)</param>
    /// <param name="min">Время начала (минуты)</param>
    /// <param name="interval">Интервал</param>
    /// <param name="timeFormat">Величина интервала</param>
    /// <param name="timerCallback">Действие</param>
    internal static Timer GetTimer(int hour, int min, double interval, TimeFormat timeFormat, TimerCallback timerCallback)
    {
        var func = GetTimeFormat(timeFormat);
        var now = DateTime.Now;
        var firstRun = new DateTime(now.Year, now.Month, now.Day, hour, min, 0, 0);
        if (now > firstRun)
        {
            firstRun = firstRun.AddDays(1);
        }

        var timeToGo = firstRun - now;
        if (timeToGo <= TimeSpan.Zero)
        {
            timeToGo = TimeSpan.Zero;
        }

        return new Timer(timerCallback, null, timeToGo, func(interval));
    }

    /// <summary>
    /// Получение таймера с проверкой условия ежедневно
    /// </summary>
    /// <param name="hour">Время начала (часы)</param>
    /// <param name="min">Время начала (минуты)</param>
    /// <param name="funcCondition">Условие</param>
    /// <param name="action">Действие</param>
    internal static Timer GetTimer(int hour, int min, Action action, Func<bool> funcCondition)
    {
        return GetTimer(hour, min, 1, TimeFormat.FromDays, x =>
        {
            if (funcCondition())
            {
                action.Invoke();
            }
        });
    }

    /// <summary>
    /// Получение таймера с проверкой условия ежедневно
    /// </summary>
    /// <param name="hour">Время начала (часы)</param>
    /// <param name="min">Время начала (минуты)</param>
    /// <param name="funcCondition">Условие</param>
    /// <param name="task">Действие</param>
    internal static Timer GetTimer(int hour, int min, Func<Task> task, Func<bool> funcCondition)
    {
        return GetTimer(hour, min, 1, TimeFormat.FromDays, async x =>
        {
            if (funcCondition())
            {
                await task.Invoke();
            }
        });
    }

    /// <summary>
    /// Получить функцию проверки дня недели
    /// </summary>
    /// <param name="daysOfWeek">Список дней недели</param>
    /// <returns></returns>
    internal static Func<bool> GetFuncCondition(DayOfWeek[] daysOfWeek)
    {
        daysOfWeek = daysOfWeek.Distinct().ToArray();
        return () => daysOfWeek.Contains(DateTime.Now.DayOfWeek);
    }

    /// <summary>
    /// Получить функцию проверки дня месяца с начала или конца
    /// </summary>
    /// <param name="daysOfMonth">Список дней месяца</param>
    /// <param name="direction">Начало отчета дней</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    internal static Func<bool> GetFuncCondition(int[] daysOfMonth, Direction direction)
    {
        daysOfMonth = daysOfMonth.Distinct().ToArray();
        return direction switch
        {
            Direction.Forwards => () => daysOfMonth.Contains(DateTime.Now.Day),
            Direction.Backwards => () => daysOfMonth.Contains(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Now.Day + 1),
            _ => throw new NotImplementedException()
        };
    }
    #endregion

    /// <summary>
    /// Получение таймера
    /// </summary>
    /// <param name="hour">Время начала (часы)</param>
    /// <param name="min">Время начала (минуты)</param>
    /// <param name="interval">Интервал</param>
    /// <param name="timeFormat">Величина интервала</param>
    /// <param name="action">Действие</param>
    public static Timer GetTimer(int hour, int min, double interval, TimeFormat timeFormat, Action action)
    {
        return GetTimer(hour, min, interval, timeFormat, x => action.Invoke());
    }

    /// <summary>
    /// Получение таймера
    /// </summary>
    /// <param name="hour">Время начала (часы)</param>
    /// <param name="min">Время начала (минуты)</param>
    /// <param name="interval">Интервал</param>
    /// <param name="timeFormat">Величина интервала</param>
    /// <param name="task">Действие</param>
    public static Timer GetTimer(int hour, int min, double interval, TimeFormat timeFormat, Func<Task> task)
    {
        return GetTimer(hour, min, interval, timeFormat, async x => await task.Invoke());
    }

    /// <summary>
    /// Получение таймера на день недели
    /// </summary>
    /// <param name="hour">Время начала (часы)</param>
    /// <param name="min">Время начала (минуты)</param>
    /// <param name="daysOfWeek">Список дней недели</param>
    /// <param name="action">Действие</param>
    public static Timer GetTimer(int hour, int min, DayOfWeek[] daysOfWeek, Action action)
    {
        return GetTimer(hour, min, action, GetFuncCondition(daysOfWeek));
    }

    /// <summary>
    /// Получение таймера на день недели
    /// </summary>
    /// <param name="hour">Время начала (часы)</param>
    /// <param name="min">Время начала (минуты)</param>
    /// <param name="daysOfWeek">Список дней недели</param>
    /// <param name="task">Действие</param>
    public static Timer GetTimer(int hour, int min, DayOfWeek[] daysOfWeek, Func<Task> task)
    {
        return GetTimer(hour, min, task, GetFuncCondition(daysOfWeek));
    }

    /// <summary>
    /// Получение таймера на день месяца
    /// </summary>
    /// <param name="hour">Время начала (часы)</param>
    /// <param name="min">Время начала (минуты)</param>
    /// <param name="daysOfMonth">Список дней месяца</param>
    /// <param name="direction">Начало отчета дней</param>
    /// <param name="action">Действие</param>
    public static Timer GetTimer(int hour, int min, int[] daysOfMonth, Direction direction, Action action)
    {
        return GetTimer(hour, min, action, GetFuncCondition(daysOfMonth, direction));
    }

    /// <summary>
    /// Получение таймера на день месяца
    /// </summary>
    /// <param name="hour">Время начала (часы)</param>
    /// <param name="min">Время начала (минуты)</param>
    /// <param name="daysOfMonth">Список дней месяца</param>
    /// <param name="direction">Начало отчета дней</param>
    /// <param name="task">Действие</param>
    public static Timer GetTimer(int hour, int min, int[] daysOfMonth, Direction direction, Func<Task> task)
    {
        return GetTimer(hour, min, task, GetFuncCondition(daysOfMonth, direction));
    }
}