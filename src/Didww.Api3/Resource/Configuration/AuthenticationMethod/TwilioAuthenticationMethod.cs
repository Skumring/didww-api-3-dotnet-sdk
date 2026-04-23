using Newtonsoft.Json;

namespace Didww.Api3.Resource.Configuration.AuthenticationMethod;

public class TwilioAuthenticationMethod : AuthenticationMethodBase
{
    [JsonIgnore]
    public override string AuthenticationType => "twilio";

    [JsonProperty("twilio_account_sid")]
    public string? TwilioAccountSid { get; set; }
}
