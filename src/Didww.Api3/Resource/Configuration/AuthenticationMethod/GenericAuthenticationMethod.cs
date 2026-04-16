using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Didww.Api3.Resource.Configuration.AuthenticationMethod;

/// <summary>
/// Fallback for unknown authentication_method types. Preserves the wire
/// type and attributes verbatim so forward-deployed SDKs can round-trip
/// newly introduced authentication methods through PATCH without data loss.
/// </summary>
public class GenericAuthenticationMethod : AuthenticationMethodBase
{
    private readonly string _storedType;

    [JsonIgnore]
    public override string AuthenticationType => _storedType;

    [JsonProperty("attributes")]
    public JObject Attributes { get; }

    public GenericAuthenticationMethod(string storedType, JObject attributes)
    {
        _storedType = storedType;
        Attributes = attributes;
    }

    // Parameterless constructor for serializer plumbing; kept internal to
    // avoid constructing a Generic without a captured type.
    internal GenericAuthenticationMethod()
    {
        _storedType = string.Empty;
        Attributes = new JObject();
    }
}
