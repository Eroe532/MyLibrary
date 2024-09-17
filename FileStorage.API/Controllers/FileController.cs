
using CaTCustomTypes.ResultType;

using FileStorage.Api.BLL;
using FileStorage.Dtos;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FileStorage.Api.Controllers;

/// <summary>
/// Контроллер
/// </summary>
[ApiController]
public abstract class FileController<TContext> : ControllerBase, IFileController where TContext : DbContext
{
    /// <summary>
    /// Репозиторий файла
    /// </summary>
    private readonly FileLogic<TContext> _logic;


    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="logic">Логика</param>
    public FileController(FileLogic<TContext> logic)
    {
        _logic = logic;
    }

    /// <summary>
    /// Вывести файл по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns></returns>
    public virtual ActionResult<FileDto?> Get([FromQuery] Guid id)
    {
        return Ok(_logic.Get(id));
    }


    /// <summary>
    /// Вывести файл по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns></returns>
    public virtual ActionResult<FileFullDto?> GetFull([FromQuery] Guid id)
    {
        return Ok(_logic.Get(id));
    }

    /// <summary>
    /// Добавление файла файла во внутреннее хранилище
    /// </summary>
    /// <param name="file">Документ</param>
    /// <returns></returns>
    public virtual ActionResult<OperationResult<Guid?>> Add([FromForm] IFormFile file)
    {
        return Add(_logic.ToFileDto(file));
    }


    /// <summary>
    /// Добавление файла файла во внутреннее хранилище
    /// </summary>
    /// <param name="documentDto">Документ</param>
    /// <returns></returns>
    public virtual ActionResult<OperationResult<Guid?>> Add([FromBody] FileSetDto documentDto)
    {
        return Ok(_logic.Create(documentDto));
    }

    /// <inheritdoc/>
    public abstract ActionResult<FileDto?> GetFile(Guid id);

    /// <inheritdoc/>
    public abstract ActionResult<FileFullDto?> GetFullFile(Guid id);

    /// <inheritdoc/>
    public abstract ActionResult<OperationResult<Guid?>> AddFile(IFormFile file);

    /// <inheritdoc/>
    public abstract ActionResult<OperationResult<Guid?>> AddFile(FileSetDto documentDto);
}
