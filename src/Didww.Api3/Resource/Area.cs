using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class Area : BaseResource
{
    public override string Type => "areas";

    public static Area Build(string id) => BaseResource.Build<Area>(id);

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("country")]
    public Country? Country { get; set; }
}
