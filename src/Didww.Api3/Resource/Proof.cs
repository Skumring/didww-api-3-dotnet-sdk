using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class Proof : BaseResource
{
    public override string Type => "proofs";

    public static Proof Build(string id) => BaseResource.Build<Proof>(id);

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("expires_at")]
    public DateTimeOffset? ExpiresAt { get; set; }

    private BaseResource? _entity;
    [JsonProperty("entity")]
    public BaseResource? Entity { get => _entity; set => _entity = MarkDirty("entity", value); }

    private ProofType? _proofType;
    [JsonProperty("proof_type")]
    public ProofType? ProofType { get => _proofType; set => _proofType = MarkDirty("proofType", value); }

    private List<EncryptedFile>? _files;
    [JsonProperty("files")]
    public List<EncryptedFile>? Files { get => _files; set => _files = MarkDirty("files", value); }
}
