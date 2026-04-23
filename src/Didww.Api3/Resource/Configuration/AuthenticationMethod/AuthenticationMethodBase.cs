using Newtonsoft.Json;

namespace Didww.Api3.Resource.Configuration.AuthenticationMethod;

/// <summary>
/// Polymorphic base for VoiceOutTrunk.authentication_method (API 2026-04-16).
/// Concrete subtypes:
///   - ip_only:             <see cref="IpOnlyAuthenticationMethod"/>
///   - credentials_and_ip:  <see cref="CredentialsAndIpAuthenticationMethod"/>
///   - twilio:              <see cref="TwilioAuthenticationMethod"/>
/// Unknown wire types are preserved via <see cref="GenericAuthenticationMethod"/>
/// (forward-compat: round-trips through PATCH without data loss).
/// </summary>
public abstract class AuthenticationMethodBase
{
    [JsonIgnore]
    public abstract string AuthenticationType { get; }
}
