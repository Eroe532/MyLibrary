namespace EventSoursing;

/// <summary>
/// Класс для формирования словаря типов
/// </summary>
public static class DictionaryType
{
    /// <summary>
    /// Создание словаря типов по заддынным пространствам имен и всех возможных тиипов
    /// рекомедую использовать Assembly.GetAssembly(MethodBase.GetCurrentMethod().DeclaringType).GetTypes() для получения всех тиипов в проекте
    /// </summary>
    /// <param name="namespaces">пространста имен</param>
    /// <param name="types">Все возможные типы 
    /// рекомедую использовать Assembly.GetAssembly(MethodBase.GetCurrentMethod().DeclaringType).GetTypes() для получения всех тиипов в проекте</param>
    /// <returns></returns>
    public static Dictionary<string, Type> GetTypes(string[] namespaces, Type[] types)
    {
        if (namespaces == null || namespaces.Length == 0)
        {
            return GetTypes(types);
        }

        if (namespaces.Length == 1)
        {
            return GetTypes(namespaces.First(), types);
        }

        var exportedTypes = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);
        foreach (var @namespace in namespaces)
        {
            foreach (var type in types)
            {
                if (type.IsClass && !string.IsNullOrEmpty(type.Namespace) && type.Namespace.Contains(@namespace, StringComparison.InvariantCultureIgnoreCase))
                {
                    exportedTypes.TryAdd(type.Name, type);
                }
            }
        }

        return exportedTypes;
    }

    /// <summary>
    /// Создание словаря типов по заддынному пространству имен и всех возможных тиипов
    /// рекомедую использовать Assembly.GetAssembly(MethodBase.GetCurrentMethod().DeclaringType).GetTypes() для получения всех тиипов в проекте
    /// </summary>
    /// <param name="namespace">пространсто имен</param>
    /// <param name="types">Все возможные типы 
    /// рекомедую использовать Assembly.GetAssembly(MethodBase.GetCurrentMethod().DeclaringType).GetTypes() для получения всех тиипов в проекте</param>
    /// <returns></returns>
    public static Dictionary<string, Type> GetTypes(string @namespace, Type[] types)
    {
        if (string.IsNullOrEmpty(@namespace))
        {
            return GetTypes(types);
        }

        var exportedTypes = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);
        foreach (var type in types)
        {
            if (type.IsClass && !string.IsNullOrEmpty(type.Namespace) && type.Namespace.Contains(@namespace, StringComparison.InvariantCultureIgnoreCase))
            {
                exportedTypes.TryAdd(type.Name, type);
            }
        }

        return exportedTypes;
    }

    /// <summary>
    /// Создание словаря типов из списка тиипов
    /// рекомедую использовать Assembly.GetAssembly(MethodBase.GetCurrentMethod().DeclaringType).GetTypes() для получения всех тиипов в проекте
    /// </summary>
    /// <param name="types">список типов 
    /// рекомедую использовать Assembly.GetAssembly(MethodBase.GetCurrentMethod().DeclaringType).GetTypes() для получения всех тиипов в проекте</param>
    /// <returns></returns>
    public static Dictionary<string, Type> GetTypes(Type[] types)
    {
        var exportedTypes = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);
        foreach (var type in types)
        {
            exportedTypes.TryAdd(type.Name, type);
        }

        return exportedTypes;
    }


    /// <summary>
    /// Создание словаря типов из списка тиипов
    /// рекомедую использовать Assembly.GetAssembly(MethodBase.GetCurrentMethod().DeclaringType).GetTypes() для получения всех тиипов в проекте
    /// </summary>
    /// <param name="namespace">пространстj имен</param>
    /// <param name="types">список типов 
    /// рекомедую использовать Assembly.GetAssembly(MethodBase.GetCurrentMethod().DeclaringType).GetTypes() для получения всех тиипов в проекте</param>
    /// <returns></returns>
    public static HashSet<string> GetEventsName(string @namespace, Type[] types)
    {
        var exportedTypes = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
        foreach (var type in types)
        {
            if (type.IsClass && !string.IsNullOrEmpty(type.Namespace) && type.Namespace.Contains(@namespace, StringComparison.InvariantCultureIgnoreCase))
            {
                exportedTypes.Add(type.Name);
            }
        }

        return exportedTypes;
    }
}
