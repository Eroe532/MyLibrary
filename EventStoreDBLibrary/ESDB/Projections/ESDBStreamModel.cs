namespace EventStoreDBLibrary.ESDB.Projections
{
    /// <summary>
    /// Модель описания потоков ESDB
    /// </summary>
    public class ESDBStreamModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ESDBStreamModel()
        {
            Title = "";
        }
    }
}