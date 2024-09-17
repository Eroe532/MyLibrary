using CaTDBContext.PostgeSQL;

using FileStorage.DAL.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileStorage.DAL;

/// <summary>
/// Контекст БД
/// </summary>
public static class AddFileStorageContext
{
    /// <summary>
    /// Настройки для модели
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="name">Название</param>
    /// <param name="schema">Схема</param>
    public static ModelBuilder AddFileInfoModelConfigure(this ModelBuilder modelBuilder, string? name = null, string schema = "public")
    {
        modelBuilder.Entity<FileInfoModel>(entity => FileInfoModelConfigure(entity, name, schema));
        return modelBuilder;
    }


    /// <summary>
    /// Настройки для модели
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="name">Название</param>
    /// <param name="schema">Схема</param>
    public static void FileInfoModelConfigure(EntityTypeBuilder<FileInfoModel> builder, string? name = null, string schema = "public")
    {
        builder.ToTableWithSchema(name, schema);
        builder.HasKey(u => u.Id);
    }

    /// <summary>
    /// Настройки для модели
    /// </summary>
    /// <param name="modelBuilder"></param>
    internal static ModelBuilder AddFileModelConfigure(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileModel>(FileModelConfigure);
        return modelBuilder;
    }

    /// <summary>
    /// Настройки для модели
    /// </summary>
    /// <param name="builder"></param>
    internal static void FileModelConfigure(EntityTypeBuilder<FileModel> builder)
    {
        builder.ToTableWithSchema();
        builder.HasKey(u => u.Id);
    }
}