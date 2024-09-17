using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using CaTCustomTypes.ResultType;

using CaTRepository.Factory;

using CaTSecurity.IdentityProvider;

using FileStorage.DAL;
using FileStorage.DAL.Model;
using FileStorage.Dtos;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FileStorage.Api.BLL;

/// <summary>
/// Логика файлового хранилища
/// </summary>
/// <typeparam name="TContext">Контекст</typeparam>
public class FileLogic<TContext> where TContext : DbContext
{
    private const string ErrorSaveFile = $"Не удалось сохранить файл";
    private const string ErrorSaveFileInfo = $"Не удалось сохранить информацию файле";
    private const string ErrorSave = $"Непредвиденая ошибка при сохранении файла";

    /// <summary>
    /// Фабрика репозиториев
    /// </summary>
    private readonly IRepositoryFactory _repositoryFactory;

    /// <summary>
    /// Максимальная величина файла
    /// </summary>
    private readonly MaximumFileSize _maximumFileSize;

    /// <summary>
    /// Логгер
    /// </summary>
    private readonly ILogger _logger;


    /// <summary>
    /// Логика файлового хранилища
    /// </summary>
    /// <param name="repositoryFactory">Фабрика репозиториев</param>
    /// <param name="maximumFileSize">Максимальная величина файла</param>
    /// <param name="logger">Логгер</param>
    public FileLogic(IRepositoryFactory repositoryFactory, MaximumFileSize maximumFileSize, ILogger<FileLogic<TContext>> logger)
    {
        _repositoryFactory = repositoryFactory;
        _logger = logger;
        _maximumFileSize = maximumFileSize;
    }

    /// <summary>
    /// Получение одного объекта по ключу
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>Один объект или пусто если нет объекта с таким ключом</returns>
    public FileDto? Get(Guid id)
    {
        var repository = _repositoryFactory.Create<FileRepository<TContext>, TContext>();

        var fileInfo = repository.Get<FileInfoModel>(id);
        return fileInfo != null ? new FileDto()
        {
            Id = fileInfo.Id,
            FileName = fileInfo.FileName
        } : null;
    }

    /// <summary>
    /// Получение одного объекта по ключу
    /// </summary>
    /// <param name="ids">Коллекция идентификаторов</param>
    /// <returns>Один объект или пусто если нет объекта с таким ключом</returns>
    public IEnumerable<FileDto> GetList(IEnumerable<Guid> ids)
    {
        var repository = _repositoryFactory.Create<FileRepository<TContext>, TContext>();

        var fileInfos = repository.GetIEnumerable<FileInfoModel>(ids);
        return fileInfos.Select(fileInfo => new FileDto()
        {
            Id = fileInfo.Id,
            FileName = fileInfo.FileName
        }).ToList();
    }

    /// <summary>
    /// Получение одного объекта по ключу
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>Один объект или пусто если нет объекта с таким ключом</returns>
    public FileFullDto? GetFull(Guid id)
    {
        var repository = _repositoryFactory.Create<FileRepository<TContext>, TContext>();

        var fileInfo = repository.Get<FileInfoModel>(id);
        if (fileInfo != null)
        {
            var fileRepository = _repositoryFactory.Create<FileRepository<FileStorageContext>, FileStorageContext>();
            var file = fileRepository.Get<FileModel>(fileInfo.DigitalBytesId);
            if (file != null)
            {
                return new FileFullDto(fileInfo.Id, fileInfo.FileName, file.DigitalBytes);
            }
        }
        return null;
    }

    /// <summary>
    /// Получение одного объекта по ключу
    /// </summary>
    /// <param name="ids">Коллекция идентификаторов</param>
    /// <returns>Один объект или пусто если нет объекта с таким ключом</returns>
    public IEnumerable<FileFullDto> GetFullList(IEnumerable<Guid> ids)
    {
        var repository = _repositoryFactory.Create<FileRepository<TContext>, TContext>();
        var fileRepository = _repositoryFactory.Create<FileRepository<FileStorageContext>, FileStorageContext>();

        var fileInfos = repository.GetIEnumerable<FileInfoModel>(ids);
        var result = new List<FileFullDto>();
        foreach (var fileInfo in fileInfos)
        {
            var file = fileRepository.Get<FileModel>(fileInfo.DigitalBytesId);
            if (file != null)
            {
                result.Add(new FileFullDto(fileInfo.Id, fileInfo.FileName, file.DigitalBytes));
            }
        }
        return result;
    }

    /// <summary>
    /// Создание объекта
    /// </summary>
    /// <param name="file">FormFile</param>
    public FileSetDto ToFileDto(IFormFile file)
    {
        using var ms = new MemoryStream();
        file.CopyTo(ms);
        return new FileSetDto()
        {
            FileName = file.FileName,
            DigitalBytes = ms.ToArray()
        };
    }

    /// <summary>
    /// Создание объекта
    /// </summary>
    /// <param name="dto">ДТО</param>
    public OperationResult<Guid?> Create(FileSetDto dto)
    {
        if (dto.DigitalBytes.LongLength > _maximumFileSize.Value)
        {
            return OperationResult.CreateErrorResult<Guid?>(new[] { $@"Размер файла превышает максимально допустимый. Загрузите файл размером не больше {_maximumFileSize.Value.GetSizeInBytesDescription()}" });
        }
        var fileRepository = _repositoryFactory.Create<FileRepository<FileStorageContext>, FileStorageContext>();
        try
        {
            var file = new FileModel(dto.DigitalBytes);
            var getFile = fileRepository.GetFile(file.Hash, file.Size);
            if (getFile != null)
            {
                file = getFile;
            }
            else
            {
                fileRepository.Create(file);
                var resSave = fileRepository.Save();
                if (resSave <= 0)
                {
                    _logger.LogError(FileLogic<TContext>.ErrorSaveFile);
                    return OperationResult.CreateErrorResult<Guid?>(new[] { FileLogic<TContext>.ErrorSaveFile });
                }
            }
            var fileInfo = new FileInfoModel()
            {
                Id = Guid.NewGuid(),
                FileName = dto.FileName,
                DigitalBytesId = file.Id,
            };
            var repository = _repositoryFactory.Create<FileRepository<TContext>, TContext>();
            repository.Create(fileInfo);
            var res = repository.Save();
            if (res <= 0)
            {
                _logger.LogError(ErrorSaveFileInfo);
                return OperationResult.CreateErrorResult<Guid?>(new[] { ErrorSaveFileInfo });
            }
            else
            {
                return OperationResult.CreateSuccessResult<Guid?>(fileInfo.Id, "Файл сохранен");
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"{ErrorSave}{Environment.NewLine}{exception.Message}");
            return OperationResult.CreateErrorResult<Guid?>(new[] { ErrorSave });
        }
    }
}
