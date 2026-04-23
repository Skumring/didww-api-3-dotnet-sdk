using Didww.Api3.Resource.Configuration.AuthenticationMethod;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Didww.Api3.Converter;

/// <summary>
/// JSON:API-style polymorphic converter for VoiceOutTrunk.authentication_method
/// (API 2026-04-16). Wraps unknown wire types in <see cref="GenericAuthenticationMethod"/>
/// so forward-deployed clients keep a consistent surface and can round-trip
/// the payload unchanged through PATCH.
///
/// CanConvert accepts the abstract base and all concrete subtypes so that
/// serialization fires regardless of the declared member type. Re-entrancy
/// (subtype attributes re-triggering the converter) is avoided by using a
/// per-thread reentrancy guard during WriteJson/ReadJson.
/// </summary>
public class AuthenticationMethodConverter : JsonConverter
{
    private static readonly Dictionary<string, Type> TypeMap = new()
    {
        ["ip_only"] = typeof(IpOnlyAuthenticationMethod),
        ["credentials_and_ip"] = typeof(CredentialsAndIpAuthenticationMethod),
        ["twilio"] = typeof(TwilioAuthenticationMethod),
    };

    [ThreadStatic]
    private static bool _reentrant;

    public override bool CanConvert(Type objectType)
    {
        if (_reentrant)
            return false;
        return typeof(AuthenticationMethodBase).IsAssignableFrom(objectType);
    }

    public override object? ReadJson(JsonReader reader, Type objectType,
        object? existingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);
        if (token.Type == JTokenType.Null)
            return null;

        var typeValue = token["type"]?.ToString();
        if (typeValue == null)
            throw new JsonSerializationException("Missing 'type' field in authentication method");

        var attributes = token["attributes"] as JObject ?? new JObject();

        if (TypeMap.TryGetValue(typeValue, out var clazz))
        {
            _reentrant = true;
            try
            {
                return attributes.ToObject(clazz, serializer);
            }
            finally
            {
                _reentrant = false;
            }
        }

        // Forward-compat: preserve the unknown type and attributes.
        return new GenericAuthenticationMethod(typeValue, attributes);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        if (value is GenericAuthenticationMethod generic)
        {
            new JObject
            {
                ["type"] = generic.AuthenticationType,
                ["attributes"] = generic.Attributes
            }.WriteTo(writer);
            return;
        }

        var method = (AuthenticationMethodBase)value;

        _reentrant = true;
        try
        {
            PolymorphicJsonHelper.Serialize(method, method.AuthenticationType, serializer).WriteTo(writer);
        }
        finally
        {
            _reentrant = false;
        }
    }
}
