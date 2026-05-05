using Didww.Api3.Internal;
using Newtonsoft.Json;

namespace Didww.Api3.Resource.Configuration.AuthenticationMethod;

/// <summary>
/// Authentication method with server-generated username/password alongside
/// allowed SIP IPs. Username and password are returned in responses only;
/// they cannot be set from the client on create.
/// </summary>
public class CredentialsAndIpAuthenticationMethod : AuthenticationMethodBase
{
    [JsonIgnore]
    public override string AuthenticationType => "credentials_and_ip";

    [JsonProperty("allowed_sip_ips")]
    public List<string>? AllowedSipIps { get; set; }

    [JsonProperty("tech_prefix")]
    public string? TechPrefix { get; set; }

    [JsonProperty("username")]
    public string? Username { get; set; }

    [JsonProperty("password")]
    public string? Password { get; set; }

    // Override ToString so default logging / debugger / error reports never
    // leak the server-generated credentials.
    public override string ToString()
    {
        var ips = AllowedSipIps == null ? "null" : $"[{string.Join(", ", AllowedSipIps)}]";
        return $"CredentialsAndIpAuthenticationMethod(AllowedSipIps={ips}, TechPrefix={TechPrefix ?? "null"}, " +
               $"Username={Redact.Mask(Username)}, Password={Redact.Mask(Password)})";
    }
}
