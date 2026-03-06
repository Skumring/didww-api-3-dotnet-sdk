using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class Country : BaseResource
{
    public override string Type => "countries";

    public static Country Build(string id) => BaseResource.Build<Country>(id);

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("prefix")]
    public string? Prefix { get; set; }

    [JsonProperty("iso")]
    public string? Iso { get; set; }

    [JsonProperty("regions")]
    public List<Region>? Regions { get; set; }
}
