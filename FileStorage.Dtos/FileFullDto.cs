using System.Net.Mime;

using MimeTypes;

namespace FileStorage.Dtos
{
    /// <summary>
    /// Файл полная информация
    /// </summary>
    public class FileFullDto : FileDto
    {
        /// <summary>
        /// Файл
        /// </summary>
        public byte[] DigitalBytes { get; set; }

        /// <summary>
        /// Content Type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Файл полная информация
        /// </summary>
        public FileFullDto()
        {
            DigitalBytes = Array.Empty<byte>();
            ContentType = string.Empty;
        }


        /// <summary>
        /// Файл полная информация
        /// </summary>
        /// <param name="id">Идетификатор</param>
        /// <param name="fileName">Название (*.*)</param>
        /// <param name="digitalBytes">Файл</param>
        public FileFullDto(Guid id, string fileName, byte[] digitalBytes)
        {
            Id = id;
            FileName = fileName;
            DigitalBytes = digitalBytes;
            ContentType = GetContentType(fileName);
        }

        /// <summary>
        /// Получить Content Type
        /// </summary>
        /// <param name="reportFileType">Возможные форматы для файлов отчета</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public void SetContentType(string reportFileType)
        {
            ContentType = GetContentType(reportFileType);
        }

        /// <summary>
        /// Получить Content Type
        /// </summary>
        /// <param name="reportFileType">Возможные форматы для файлов отчета</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string GetContentType(string reportFileType)
        {
            var fileFormat = reportFileType.Split('.').Last().ToLower();
            return MimeTypeMap.GetMimeType(fileFormat);
        }
    }
}
