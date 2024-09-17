using CaTCustomTypes.ResultType;

using FileStorage.Dtos;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.Api.Controllers;

/// <summary>
/// Контроллер
/// </summary>
public interface IFileController
{
    /// <summary>
    /// Вывести файл по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns></returns>
    public ActionResult<FileDto?> GetFile(Guid id);

    /// <summary>
    /// Вывести файл по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns></returns>
    public ActionResult<FileFullDto?> GetFullFile(Guid id); 

    /// <summary>
    /// Добавление файла файла во внутреннее хранилище
    /// </summary>
    /// <param name="file">Документ</param>
    /// <returns></returns>
    public ActionResult<OperationResult<Guid?>> AddFile(IFormFile file);


    /// <summary>
    /// Добавление файла файла во внутреннее хранилище
    /// </summary>
    /// <param name="documentDto">Документ</param>
    /// <returns></returns>
    public ActionResult<OperationResult<Guid?>> AddFile(FileSetDto documentDto);
}