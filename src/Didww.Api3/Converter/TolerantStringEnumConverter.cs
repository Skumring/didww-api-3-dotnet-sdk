using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Didww.Api3.Converter;

/// <summary>
/// A JSON converter for string-backed enums that gracefully handles unknown values
/// from the server instead of throwing <see cref="JsonSerializationException"/>.
/// When an unrecognized value is encountered, it falls back to the first defined
/// member of the enum (index 0).
/// </summary>
public class TolerantStringEnumConverter : StringEnumConverter
{
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        try
        {
            return base.ReadJson(reader, objectType, existingValue, serializer);
        }
        catch (JsonSerializationException)
        {
            var enumType = Nullable.GetUnderlyingType(objectType) ?? objectType;
            var values = Enum.GetValues(enumType);
            if (values.Length > 0)
                return values.GetValue(0);
            return existingValue;
        }
    }
}
