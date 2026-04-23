using Newtonsoft.Json;

namespace Didww.Api3.Resource.OrderItem;

public class EmergencyOrderItem : OrderItemBase
{
    [JsonIgnore]
    public override string ItemType => "emergency_order_items";

    [JsonProperty("qty")]
    public int? Qty { get; set; }

    [JsonProperty("emergency_calling_service_id")]
    public string? EmergencyCallingServiceId { get; set; }

    [JsonProperty("nrc")]
    public string? Nrc { get; set; }

    [JsonProperty("mrc")]
    public string? Mrc { get; set; }

    [JsonProperty("prorated_mrc")]
    public bool? ProratedMrc { get; set; }

    [JsonProperty("billed_from")]
    public string? BilledFrom { get; set; }

    [JsonProperty("billed_to")]
    public string? BilledTo { get; set; }
}
