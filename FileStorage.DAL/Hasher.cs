using System.Security.Cryptography;

namespace FileStorage.DAL;

/// <summary>
/// Методы расширения для Хеширования
/// </summary>
public static class Hasher
{
    /// <summary>
    /// Хеш для System.Array
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int Hash(this byte[] obj)
    {
        return BitConverter.ToInt32(SHA256.HashData(obj), 0);
    }

    /// <summary>
    /// Хеш для IEnumerable
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int Hash(this IEnumerable<byte> obj)
    {
        return obj.ToArray().Hash();
    }
}
