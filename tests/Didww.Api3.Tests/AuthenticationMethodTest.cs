using Didww.Api3.Converter;
using Didww.Api3.Resource.Configuration.AuthenticationMethod;
using FluentAssertions;
using Newtonsoft.Json;

namespace Didww.Api3.Tests;

public class AuthenticationMethodTest
{
    private static readonly JsonSerializerSettings Settings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        Converters = { new AuthenticationMethodConverter() }
    };

    [Fact]
    public void Deserializes_IpOnly_Wire_To_IpOnly_Subtype()
    {
        var json = @"{""type"":""ip_only"",""attributes"":{""allowed_sip_ips"":[""203.0.113.1/32""],""tech_prefix"":""""}}";

        var obj = JsonConvert.DeserializeObject<AuthenticationMethodBase>(json, Settings);

        obj.Should().BeOfType<IpOnlyAuthenticationMethod>();
        var ipOnly = (IpOnlyAuthenticationMethod)obj!;
        ipOnly.AllowedSipIps.Should().ContainSingle().Which.Should().Be("203.0.113.1/32");
        ipOnly.TechPrefix.Should().Be("");
        ipOnly.AuthenticationType.Should().Be("ip_only");
    }

    [Fact]
    public void Deserializes_CredentialsAndIp_Wire_To_CredentialsAndIp_Subtype()
    {
        var json = @"{""type"":""credentials_and_ip"",""attributes"":{""allowed_sip_ips"":[""203.0.113.1/32""],""username"":""u"",""password"":""p"",""tech_prefix"":""9""}}";

        var obj = JsonConvert.DeserializeObject<AuthenticationMethodBase>(json, Settings);

        obj.Should().BeOfType<CredentialsAndIpAuthenticationMethod>();
        var cai = (CredentialsAndIpAuthenticationMethod)obj!;
        cai.Username.Should().Be("u");
        cai.Password.Should().Be("p");
        cai.AllowedSipIps.Should().ContainSingle().Which.Should().Be("203.0.113.1/32");
        cai.TechPrefix.Should().Be("9");
        cai.AuthenticationType.Should().Be("credentials_and_ip");
    }

    [Fact]
    public void Deserializes_Twilio_Wire_To_Twilio_Subtype()
    {
        var json = @"{""type"":""twilio"",""attributes"":{""twilio_account_sid"":""AC123""}}";

        var obj = JsonConvert.DeserializeObject<AuthenticationMethodBase>(json, Settings);

        obj.Should().BeOfType<TwilioAuthenticationMethod>();
        var twilio = (TwilioAuthenticationMethod)obj!;
        twilio.TwilioAccountSid.Should().Be("AC123");
        twilio.AuthenticationType.Should().Be("twilio");
    }

    [Fact]
    public void Deserializes_Unknown_Type_To_Generic_With_Preserved_Type_And_Attributes()
    {
        var json = @"{""type"":""future_auth_method"",""attributes"":{""some_new_field"":""value"",""another"":42}}";

        var obj = JsonConvert.DeserializeObject<AuthenticationMethodBase>(json, Settings);

        obj.Should().BeOfType<GenericAuthenticationMethod>();
        var generic = (GenericAuthenticationMethod)obj!;
        generic.AuthenticationType.Should().Be("future_auth_method");
        generic.Attributes.Should().ContainKey("some_new_field");
        generic.Attributes["some_new_field"]!.ToString().Should().Be("value");
        generic.Attributes.Should().ContainKey("another");
        generic.Attributes["another"]!.ToObject<int>().Should().Be(42);
    }

    [Fact]
    public void Serializes_IpOnly_As_Wire_Shape_With_Singular_Type()
    {
        var ipOnly = new IpOnlyAuthenticationMethod
        {
            AllowedSipIps = new List<string> { "203.0.113.1/32" },
            TechPrefix = ""
        };

        var json = JsonConvert.SerializeObject(ipOnly, Settings);

        json.Should().Be(@"{""type"":""ip_only"",""attributes"":{""allowed_sip_ips"":[""203.0.113.1/32""],""tech_prefix"":""""}}");
    }

    [Fact]
    public void Roundtrips_Unknown_Type_Through_Generic_Without_Data_Loss()
    {
        var inputJson = @"{""type"":""future_auth_method"",""attributes"":{""some_new_field"":""value"",""another"":42}}";

        var obj = JsonConvert.DeserializeObject<AuthenticationMethodBase>(inputJson, Settings);
        var outputJson = JsonConvert.SerializeObject(obj, Settings);

        outputJson.Should().Be(inputJson);
    }
}
