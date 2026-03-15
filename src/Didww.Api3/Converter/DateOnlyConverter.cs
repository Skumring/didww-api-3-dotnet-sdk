using System.Globalization;
using Newtonsoft.Json;

namespace Didww.Api3.Converter;

public class DateOnlyConverter : JsonConverter<DateOnly?>
{
    public override DateOnly? ReadJson(JsonReader reader, Type objectType, DateOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;

        return DateOnly.ParseExact((string)reader.Value!, "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    public override void WriteJson(JsonWriter writer, DateOnly? value, JsonSerializer serializer)
    {
        if (value is null)
            writer.WriteNull();
        else
            writer.WriteValue(value.Value.ToString("yyyy-MM-dd"));
    }
}
