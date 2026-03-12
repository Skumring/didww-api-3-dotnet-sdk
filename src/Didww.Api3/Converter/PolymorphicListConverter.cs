using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Didww.Api3.Converter;

public abstract class PolymorphicListConverter<T> : JsonConverter<List<T>>
    where T : class
{
    protected abstract Dictionary<string, Type> TypeMap { get; }
    protected abstract string TypeName { get; }
    protected abstract string GetItemType(T item);

    public override List<T>? ReadJson(JsonReader reader, Type objectType, List<T>? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);
        if (token.Type == JTokenType.Null)
            return null;

        var items = new List<T>();
        if (token.Type == JTokenType.Array)
        {
            foreach (var itemNode in token)
            {
                var item = PolymorphicJsonHelper.Deserialize<T>(itemNode, TypeMap, TypeName, serializer);
                if (item != null)
                    items.Add(item);
            }
        }
        return items;
    }

    public override void WriteJson(JsonWriter writer, List<T>? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartArray();
        foreach (var item in value)
            PolymorphicJsonHelper.Serialize(item, GetItemType(item), serializer).WriteTo(writer);
        writer.WriteEndArray();
    }
}
