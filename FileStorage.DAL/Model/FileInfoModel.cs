using BaseEntities.Interfaces;

namespace FileStorage.DAL.Model;

/// <summary>
/// Модель таблицы файлы
/// </summary>
public class FileInfoModel : IIdentityEntity<Guid>
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
    /// Файл
    /// </summary>
    public Guid DigitalBytesId { get; set; }

    /// <summary>
    /// Конструктор
    /// </summary>
    public FileInfoModel()
    {
        FileName = "";
    }
}
