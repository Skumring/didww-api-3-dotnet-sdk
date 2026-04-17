using Didww.Api3.Resource.Enums;
using Newtonsoft.Json;

namespace Didww.Api3.Resource;

/// <summary>
/// Read-only DID history resource (API 2026-04-16).
/// When <see cref="Action"/> is <c>billing_cycles_count_changed</c>,
/// the JSON:API meta block contains <c>from</c> and <c>to</c> integer
/// fields representing the previous and new billing cycle counts.
/// Those meta keys are absent for all other action types.
/// </summary>
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

    /// <summary>
    /// Resource-level meta from the JSON:API response.
    /// When <see cref="Action"/> is <see cref="DidHistoryAction.BillingCyclesCountChanged"/>,
    /// contains <c>from</c> and <c>to</c> integer keys representing the previous
    /// and new billing cycle counts. Absent for all other action types.
    /// </summary>
    [JsonProperty("meta")]
    public Dictionary<string, object>? Meta { get; set; }
}
