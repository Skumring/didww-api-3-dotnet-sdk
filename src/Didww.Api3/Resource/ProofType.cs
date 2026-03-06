using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class ProofType : BaseResource
{
    public override string Type => "proof_types";
    public static ProofType Build(string id) => BaseResource.Build<ProofType>(id);

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("entity_type")]
    public string? EntityType { get; set; }
}
