using Didww.Api3.Resource.Enums;
using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class DidHistory : BaseResource
{
    public override string Type => "did_history";

    [JsonProperty("did_number")]
    public string? DidNumber { get; set; }

    [JsonProperty("action")]
    public DidHistoryAction? Action { get; set; }

    [JsonProperty("method")]
    public DidHistoryMethod? Method { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }
}
