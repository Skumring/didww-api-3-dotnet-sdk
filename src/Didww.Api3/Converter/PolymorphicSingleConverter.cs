using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Didww.Api3.Converter;

public abstract class PolymorphicSingleConverter<T> : JsonConverter<T>
    where T : class
{
    protected abstract Dictionary<string, Type> TypeMap { get; }
    protected abstract string TypeName { get; }
    protected abstract string GetItemType(T item);

    public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);
        if (token.Type == JTokenType.Null)
            return null;

        return PolymorphicJsonHelper.Deserialize<T>(token, TypeMap, TypeName, serializer);
    }

    public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        PolymorphicJsonHelper.Serialize(value, GetItemType(value), serializer).WriteTo(writer);
    }
}
