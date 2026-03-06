using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Didww.Api3.Converter;

internal static class PolymorphicJsonHelper
{
    public static T? Deserialize<T>(JToken node, Dictionary<string, Type> typeMap, string entityName, JsonSerializer serializer)
    {
        var typeValue = node["type"]?.ToString();
        if (typeValue == null)
            throw new JsonSerializationException($"Missing 'type' field in {entityName}");

        if (!typeMap.TryGetValue(typeValue, out var clazz))
            return default;

        var attributes = node["attributes"];
        if (attributes == null)
            return (T?)Activator.CreateInstance(clazz);

        return (T?)attributes.ToObject(clazz, serializer);
    }

    public static JObject Serialize(object value, string type, JsonSerializer serializer)
    {
        var wrapper = new JObject
        {
            ["type"] = type,
            ["attributes"] = JObject.FromObject(value, serializer)
        };
        return wrapper;
    }
}
