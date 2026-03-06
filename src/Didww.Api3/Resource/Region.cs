using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class Region : BaseResource
{
    public override string Type => "regions";

    public static Region Build(string id) => BaseResource.Build<Region>(id);

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("iso")]
    public string? Iso { get; set; }

    [JsonProperty("country")]
    public Country? Country { get; set; }
}
