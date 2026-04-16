using Newtonsoft.Json;

namespace Didww.Api3.Resource.Configuration.AuthenticationMethod;

/// <summary>
/// Read-only authentication method that restricts access by IP address only.
/// This method can only be configured manually by DIDWW staff upon request
/// and cannot be set via the API on create or update.
/// Trunks that already have ip_only authentication can still be read and
/// their non-authentication attributes (e.g. name, rtp_timeout) updated normally.
/// </summary>
public class IpOnlyAuthenticationMethod : AuthenticationMethodBase
{
    [JsonIgnore]
    public override string AuthenticationType => "ip_only";

    [JsonProperty("allowed_sip_ips")]
    public List<string>? AllowedSipIps { get; set; }

    [JsonProperty("tech_prefix")]
    public string? TechPrefix { get; set; }
}
