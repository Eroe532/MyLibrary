namespace EventStoreDBLibrary.ESDB.Projections
{
    /// <summary>
    /// Класс конфигураций для соотношения его с json файлом
    /// </summary>
    public class ESDBConfModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Были ли изменения (true изменения применятся и выставится false)
        /// </summary>
        public bool IsChange { get; set; }

        /// <summary>
        /// Дата применения
        /// </summary>
        public DateTime ChangeDate { get; set; }

        /// <summary>
        /// Список Проджекшинов
        /// </summary>
        public List<ESDBProjectionModel> ListProjection { get; set; } = new List<ESDBProjectionModel>();

        /// <summary>
        /// Список потоков
        /// </summary>
        public List<ESDBStreamModel> ListStream { get; set; } = new List<ESDBStreamModel>();
    }
}