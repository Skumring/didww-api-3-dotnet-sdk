using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class Pop : BaseResource
{
    public override string Type => "pops";

    public static Pop Build(string id) => BaseResource.Build<Pop>(id);

    [JsonProperty("name")]
    public string? Name { get; set; }
}
