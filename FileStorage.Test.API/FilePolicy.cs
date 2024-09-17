using CaTSecurity;

namespace FileStorage.Test.API;

/// <summary>
/// Политики безопасности
/// </summary>
public class FilePolicy : PolicySettings
{
    /// <summary>
    /// Политики безопасности
    /// </summary>
    public FilePolicy()
    {
        AddPolicy("FileRead", "File", "FileRead");
        AddPolicy("FileWrite", "File", "FileWrite");


    }

}