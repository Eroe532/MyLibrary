namespace EventStoreDBLibrary.ESDB.Projections
{
    /// <summary>
    /// Класс проджекшина
    /// </summary>
    public class ESDBProjectionModel
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ESDBProjectionModel()
        {
            NameProjection = "";
            JsBodyProjection = "";
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название проджекшина
        /// </summary>
        public string NameProjection { get; set; }

        /// <summary>
        /// Код проджекшина
        /// </summary>
        public string JsBodyProjection { get; set; }

    }
}