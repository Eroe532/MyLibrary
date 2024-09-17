using CaTCommonDto;

using CaTCustomTypes.ResultType;

using FileStorage.Api.BLL;
using FileStorage.Api.Controllers;
using FileStorage.Dtos;
using FileStorage.Test.API;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FileStorage.Test.API.Controllers;

/// <summary>
/// Контроллер
/// </summary>
[ApiController]
public abstract class FileController : FileController<FileInfoContext>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="logic">Логика</param>
    public FileController(FileLogic<FileInfoContext> logic) : base(logic)
    {
    }
    /// <inheritdoc/>
    public override ActionResult<FileDto?> GetFile(Guid id)
    {
        return Get(id);
    }

    /// <inheritdoc/>
    public override ActionResult<FileFullDto?> GetFullFile(Guid id)
    {
        return GetFull(id);
    }

    /// <inheritdoc/>
    public override ActionResult<OperationResult<Guid?>> AddFile(IFormFile file)
    {
        return AddFile(file);
    }

    /// <inheritdoc/>
    public override ActionResult<OperationResult<Guid?>> AddFile(FileSetDto fileDto)
    {
        return AddFile(fileDto);
    }
}
