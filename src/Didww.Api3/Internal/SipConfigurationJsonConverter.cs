using Didww.Api3.Resource.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Didww.Api3.Internal;

/// <summary>
/// Applies wire-time cascade rules and read-only field stripping to
/// <see cref="SipConfiguration"/> on serialize. The class itself stays a
/// plain POCO — there are no flag fields, ShouldSerialize hooks, or
/// setter side-effects. Server-enforced multi-field invariants live
/// here in one place and read like a contract.
///
/// Cascade rules (API 2026-04-16):
///   - EnabledSipRegistration == true  ->  emit "host":null and
///                                          "port":null on the wire
///   - EnabledSipRegistration == false ->  force "use_did_in_ruri":false
///   - Host non-blank                  ->  force "enabled_sip_registration":false
///                                          and "use_did_in_ruri":false
///
/// Deserialization is delegated back to the default contract — server
/// responses are already internally consistent and the cascade must not
/// run against them.
/// </summary>
internal class SipConfigurationJsonConverter : JsonConverter<SipConfiguration>
{
    // Inner serializer used to dump SipConfiguration to a JObject without
    // re-entering this converter. The contract resolver below clears the
    // class-level [JsonConverter] binding on SipConfiguration so the inner
    // serializer falls through to default reflection-based serialization.
    private static readonly JsonSerializer _innerSerializer = JsonSerializer.Create(
        new JsonSerializerSettings
        {
            ContractResolver = new BypassConverterContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
        });

    public override bool CanRead => false;

    public override SipConfiguration ReadJson(
        JsonReader reader,
        System.Type objectType,
        SipConfiguration? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer) =>
        // CanRead == false routes deserialization through the default
        // contract; this method is unreachable.
        throw new System.NotSupportedException();

    public override void WriteJson(JsonWriter writer, SipConfiguration? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        var json = (JObject)JToken.FromObject(value, _innerSerializer);

        // Strip read-only fields. The server rejects writes with
        // 400 Param not allowed.
        json.Remove("incoming_auth_username");
        json.Remove("incoming_auth_password");

        // Cascade. EnabledSipRegistration == true takes precedence over a
        // user-set Host: a caller that explicitly turns SIP registration on
        // wants the server to clear any pre-existing host/port even if
        // they've also assigned Host on the in-memory config.
        if (value.EnabledSipRegistration == true)
        {
            json["host"] = JValue.CreateNull();
            json["port"] = JValue.CreateNull();
        }
        else
        {
            if (value.EnabledSipRegistration == false)
            {
                json["use_did_in_ruri"] = false;
            }

            if (!string.IsNullOrEmpty(value.Host))
            {
                json["enabled_sip_registration"] = false;
                json["use_did_in_ruri"] = false;
            }
        }

        json.WriteTo(writer);
    }

    private class BypassConverterContractResolver : DefaultContractResolver
    {
        protected override JsonContract CreateContract(System.Type objectType)
        {
            var contract = base.CreateContract(objectType);
            if (objectType == typeof(SipConfiguration))
            {
                // Clear the class-level [JsonConverter] binding so the
                // inner serializer doesn't recurse back into us.
                contract.Converter = null;
            }
            return contract;
        }
    }
}
