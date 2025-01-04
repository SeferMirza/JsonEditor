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
                var list = new List<object>();
                foreach (var item in property.Value.EnumerateArray())
                {
                    list.Add(item.ValueKind == JsonValueKind.Object ? ParseToDictionary(item) : item.GetRawText());
                }
                dictionary[property.Name] = list;
            }
            else
            {
                dictionary[property.Name] = property.Value.GetRawText();
            }
        }

        return dictionary;
    }
}
