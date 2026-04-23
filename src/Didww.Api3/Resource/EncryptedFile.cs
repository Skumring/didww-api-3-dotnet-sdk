using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class EncryptedFile : BaseResource
{
    public override string Type => "encrypted_files";
    public static EncryptedFile Build(string id) => BaseResource.Build<EncryptedFile>(id);

    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("expires_at")] public DateTimeOffset? ExpiresAt { get; set; }
}
