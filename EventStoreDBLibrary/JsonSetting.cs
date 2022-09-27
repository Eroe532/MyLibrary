using System.Text.Json;

namespace EventStoreDBLibrary
{
    /// <summary>
    /// Класс с настройками Json сериализации
    /// </summary>
    public class JsonSetting
    {
        /// <summary>
        /// Настройки сериализации: 
        /// Учет Регистра, 
        /// Вчлючение полей
        /// </summary>
        static public JsonSerializerOptions NameCaseInsensitiveIncludeFields { get; } = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, IncludeFields = true };

        /// <summary>
        /// Настройки сериализации: 
        /// Учет Регистра
        /// </summary>
        static public JsonSerializerOptions NameCaseInsensitive { get; } = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        /// <summary>
        /// Настройки сериализации: 
        /// Добавление лишних пробелов
        /// </summary>
        static public JsonSerializerOptions WriteIndented { get; } = new JsonSerializerOptions() { WriteIndented = true };
    }
}