using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class DidGroupType : BaseResource
{
    public override string Type => "did_group_types";

    public static DidGroupType Build(string id) => BaseResource.Build<DidGroupType>(id);

    [JsonProperty("name")]
    public string? Name { get; set; }
}
