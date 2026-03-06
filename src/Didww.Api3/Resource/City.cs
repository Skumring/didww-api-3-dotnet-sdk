using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class City : BaseResource
{
    public override string Type => "cities";

    public static City Build(string id) => BaseResource.Build<City>(id);

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("country")]
    public Country? Country { get; set; }

    [JsonProperty("region")]
    public Region? Region { get; set; }

    [JsonProperty("area")]
    public Area? Area { get; set; }
}
