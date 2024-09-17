using AutoMapper;

using CCMApi.DAL.ReadModels.Repository;

using FileStorage.DAL.Model;

using Microsoft.EntityFrameworkCore;

namespace FileStorage.DAL;

/// <summary>
/// Репозиторий для взаимодействия с таблицей файлы
/// </summary>
public class FileRepository<TContext> : BaseRepository<TContext> where TContext : DbContext
{
    /// <summary>
    /// Констуктор
    /// </summary>
    /// <param name="context">Контекст БД</param>
    /// <param name="mapper">Mapper</param>
    public FileRepository(TContext context, IMapper mapper) : base(context, mapper)
    {
    }

    /// <summary>
    /// Получить Файл
    /// </summary>
    /// <param name="hash">Хэш</param>
    /// <param name="size">Размер</param>
    /// <returns></returns>
    public FileModel? GetFile(int hash, int size)
    {
        return Get<FileModel>(m => m.Hash == hash && m.Size == size);
    }
}

