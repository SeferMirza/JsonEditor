using System.Text.Json;

namespace JsonEditor;

public static class JsonExtensions
{
    public static Dictionary<string, object> ParseToDictionary(this JsonElement element)
    {
        var dictionary = new Dictionary<string, object>();

        foreach (var property in element.EnumerateObject())
        {
            if (property.Value.ValueKind == JsonValueKind.Object)
            {
                dictionary[property.Name] = ParseToDictionary(property.Value);
            }
            else if (property.Value.ValueKind == JsonValueKind.Array)
            {
                dictionary[property.Name] = ParseArray(property.Value);
            }
            else
            {
                dictionary[property.Name] = GetJsonValue(property.Value);
            }
        }

        return dictionary;
    }

    static object ParseArray(JsonElement arrayElement)
    {
        var list = new List<object>();

        foreach (var item in arrayElement.EnumerateArray())
        {
            if (item.ValueKind == JsonValueKind.Object)
            {
                list.Add(ParseToDictionary(item));
            }
            else
            {
                list.Add(GetJsonValue(item));
            }
        }

        if (list.All(x => x is string))
        {
            return list.Cast<string>().ToList();
        }
        else if (list.All(x => x is int))
        {
            return list.Cast<int>().ToList();
        }
        else
        {
            return list;
        }
    }

    static object GetJsonValue(JsonElement element)
    {
        object? value = element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number when element.TryGetInt32(out int intValue) => intValue,
            JsonValueKind.Number => element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            _ => element.GetRawText(),
        };
        return value!;
    }

    public static string GetFriendlyTypeName(this Type type)
    {
        if (type.IsGenericType)
        {
            string typeName = type.Name.Split('`')[0];
            string genericArgs = string.Join(", ", type.GenericTypeArguments.Select(GetFriendlyTypeName));
            return $"{typeName}<{genericArgs}>";
        }

        if (type.IsArray)
        {
            return $"{type.GetElementType()?.GetFriendlyTypeName()}[]";
        }

        return type.Name;
    }
}
