using BaseEntities.Interfaces;

namespace FileStorage.DAL.Model;

/// <summary>
/// Модель таблицы файлы
/// </summary>
public class FileModel : IIdentityEntity<Guid>
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Хэш
    /// </summary>
    public int Hash { get; set; }

    /// <summary>
    /// Размен массива
    /// </summary>
    public int Size { get; set; }

    /// <summary>
    /// Файл
    /// </summary>
    public byte[] DigitalBytes { get; set; }

    /// <summary>
    /// Конструктор
    /// </summary>
    public FileModel()
    {
        DigitalBytes = Array.Empty<byte>();
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="digitalBytes">Массив битов</param>
    public FileModel(byte[] digitalBytes)
    {
        Id = Guid.NewGuid();
        DigitalBytes = digitalBytes;
        Hash = DigitalBytes.Hash();
        Size = DigitalBytes.Length;
    }
}
