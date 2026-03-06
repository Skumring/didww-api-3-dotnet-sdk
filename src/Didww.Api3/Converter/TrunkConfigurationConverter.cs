using Didww.Api3.Resource.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Didww.Api3.Converter;

public class TrunkConfigurationConverter : JsonConverter<TrunkConfiguration>
{
    private static readonly Dictionary<string, Type> TypeMap = new()
    {
        ["sip_configurations"] = typeof(SipConfiguration),
        ["pstn_configurations"] = typeof(PstnConfiguration),
    };

    public override TrunkConfiguration? ReadJson(JsonReader reader, Type objectType, TrunkConfiguration? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);
        if (token.Type == JTokenType.Null)
            return null;

        return PolymorphicJsonHelper.Deserialize<TrunkConfiguration>(token, TypeMap, "trunk configuration", serializer);
    }

    public override void WriteJson(JsonWriter writer, TrunkConfiguration? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        var wrapper = PolymorphicJsonHelper.Serialize(value, value.ConfigurationType, serializer);
        wrapper.WriteTo(writer);
    }
}
