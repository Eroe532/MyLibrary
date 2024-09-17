namespace FileStorage.Dtos
{
    /// <summary>
    /// Файл
    /// </summary>
    public class FileDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название (*.*)
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public FileDto()
        {
            FileName = "";
        }
    }
}
