using FileStorage.DAL;

using Microsoft.EntityFrameworkCore;

namespace FileStorage.Test.API;

/// <summary>
/// Контекст БД
/// </summary>
public class FileInfoContext : DbContext
{
    /// <summary>
    /// Контекст БД
    /// </summary>
    /// <param name="options">Опции</param>
    public FileInfoContext(DbContextOptions<FileInfoContext> options) : base(options)
    {
    }

    /// <summary>
    /// При создании моделей
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddFileInfoModelConfigure();
    }
}
