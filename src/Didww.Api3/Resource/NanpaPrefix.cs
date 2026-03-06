using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class NanpaPrefix : BaseResource
{
    public override string Type => "nanpa_prefixes";

    public static NanpaPrefix Build(string id) => BaseResource.Build<NanpaPrefix>(id);

    [JsonProperty("npa")]
    public string? Npa { get; set; }

    [JsonProperty("nxx")]
    public string? Nxx { get; set; }

    [JsonProperty("country")]
    public Country? Country { get; set; }

    [JsonProperty("region")]
    public Region? Region { get; set; }
}
