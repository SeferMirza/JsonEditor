using System.IO;
using System.Text.Json;

namespace JsonEditor;

public class JsonService
{
    Dictionary<string, object> _json = [];
    object _selectedObject;
    List<string> _selectedPath = [];

    public Dictionary<string, object> GetCurrentJson => _json;

    public Dictionary<string, object> OpenJson(string filename)
    {
        using StreamReader r = new(filename);
        string jsonString = r.ReadToEnd();

        using var document = JsonDocument.Parse(jsonString);
        var elements = document.RootElement.ParseToDictionary();

        _json = elements;

        return elements;
    }

    public (string key, object value) SelectProperty(List<string> path)
    {
        _selectedPath = path;
        _selectedObject  = TravelOnJson(_json, path);
        
        return (path.Last(), _selectedObject);
    }

    object TravelOnJson(Dictionary<string, object> dic, List<string> path, int index = 0)
    {
        if (path.Count - 1 == index)
        {
            return dic[path[index]];
        }

        if (dic[path[index]] is Dictionary<string, object> dicProperty)
        {
            return TravelOnJson(dicProperty, path, index + 1);
        }

        return null; // TODO ?
    }

    public void UpdateProperty(string propertyName)
    {
        var currentDict = _json;

        for (int i = 0; i < _selectedPath.Count - 1; i++)
        {
            if (currentDict.TryGetValue(_selectedPath[i], out var value) && value is Dictionary<string, object> nestedDict)
            {
                currentDict = nestedDict;
            }
        }

        var lastKey = _selectedPath[^1];
        if (currentDict.TryGetValue(lastKey, out object? valueToMove))
        {
            currentDict.Remove(lastKey);
            currentDict[propertyName] = valueToMove;
        }
    }

    public void UpdateJson() { }
}
