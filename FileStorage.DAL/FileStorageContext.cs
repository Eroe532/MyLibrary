using Microsoft.EntityFrameworkCore;

namespace FileStorage.DAL;

/// <summary>
/// Контекст БД
/// </summary>
public class FileStorageContext : DbContext
{
    /// <summary>
    /// Контекст БД
    /// </summary>
    /// <param name="options">Опции</param>
    public FileStorageContext(DbContextOptions<FileStorageContext> options) : base(options)
    {
    }

    /// <summary>
    /// При создании моделей
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddFileModelConfigure();
    }
}
