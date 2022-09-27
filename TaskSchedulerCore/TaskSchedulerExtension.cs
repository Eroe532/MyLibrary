namespace TaskSchedulerCore
{
    /// <summary>
    /// Расширения и статичные функции
    /// </summary>
    public static class TaskSchedulerExtension
    {
        /// <summary>
        /// Получение функции формирования интервала
        /// </summary>
        /// <param name="timeFormat">Величина интервала</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal static Func<double, TimeSpan> GetTimeFormat(this TimeFormat timeFormat)
        {
            return timeFormat switch
            {
                TimeFormat.FromDays => TimeSpan.FromDays,
                TimeFormat.FromHours => TimeSpan.FromHours,
                TimeFormat.FromMinutes => TimeSpan.FromMinutes,
                TimeFormat.FromSeconds => TimeSpan.FromSeconds,
                TimeFormat.FromMilliseconds => TimeSpan.FromMilliseconds,
                _ => throw new Exception()
            };
        }


        /// <summary>
        /// Получение таймера
        /// </summary>
        /// <param name="hour">Время начала (часы)</param>
        /// <param name="min">Время начала (минуты)</param>
        /// <param name="interval">Интервал</param>
        /// <param name="timeFormat">Величина интервала</param>
        /// <param name="action">Действие</param>
        public static Timer GetTimer((int hour, int min) time, double interval, TimeFormat timeFormat, Action action)
        {
            var func = GetTimeFormat(timeFormat);
            var now = DateTime.Now;
            var firstRun = new DateTime(now.Year, now.Month, now.Day, time.hour, time.min, 0, 0);
            if (now > firstRun)
            {
                firstRun = firstRun.AddDays(1);
            }

            var timeToGo = firstRun - now;
            if (timeToGo <= TimeSpan.Zero)
            {
                timeToGo = TimeSpan.Zero;
            }

            return new Timer(x =>
            {
                action.Invoke();
            }, null, timeToGo, func(interval));
        }

        /// <summary>
        /// Получение таймера
        /// </summary>
        /// <param name="hour">Время начала (часы)</param>
        /// <param name="min">Время начала (минуты)</param>
        /// <param name="interval">Интервал</param>
        /// <param name="timeFormat">Величина интервала</param>
        /// <param name="task">Действие</param>
        public static Timer GetTimer((int hour, int min) time, double interval, TimeFormat timeFormat, Func<Task> task)
        {
            var func = timeFormat.GetTimeFormat();
            var now = DateTime.Now;
            var firstRun = new DateTime(now.Year, now.Month, now.Day, time.hour, time.min, 0, 0);
            if (now > firstRun)
            {
                firstRun = firstRun.AddDays(1);
            }

            var timeToGo = firstRun - now;
            if (timeToGo <= TimeSpan.Zero)
            {
                timeToGo = TimeSpan.Zero;
            }

            return new Timer(async x =>
            {
                await task.Invoke();
            }, null, timeToGo, func(interval));
        }
    }
}