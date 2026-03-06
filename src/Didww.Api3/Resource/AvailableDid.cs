using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class AvailableDid : BaseResource
{
    public override string Type => "available_dids";

    public static AvailableDid Build(string id) => BaseResource.Build<AvailableDid>(id);

    [JsonProperty("number")]
    public string? Number { get; set; }

    [JsonProperty("did_group")]
    public DidGroup? DidGroup { get; set; }

    [JsonProperty("nanpa_prefix")]
    public NanpaPrefix? NanpaPrefix { get; set; }
}
