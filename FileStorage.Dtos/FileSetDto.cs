namespace FileStorage.Dtos
{
    /// <summary>
    /// Файл
    /// </summary>
    public class FileSetDto
    {
        /// <summary>
        /// Название (*.*)
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public byte[] DigitalBytes { get; set; }


        /// <summary>
        /// Конструктор
        /// </summary>
        public FileSetDto()
        {
            FileName = "";
            DigitalBytes = Array.Empty<byte>();
        }
    }
}
