using Didww.Api3.Resource.Enums;
using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class EmergencyVerification : BaseResource
{
    public override string Type => "emergency_verifications";

    public static EmergencyVerification Build(string id) => BaseResource.Build<EmergencyVerification>(id);

    [JsonProperty("reference")]
    public string? Reference { get; set; }

    [JsonProperty("status")]
    public EmergencyVerificationStatus? Status { get; set; }

    [JsonProperty("reject_reasons")]
    public List<string>? RejectReasons { get; set; }

    [JsonProperty("reject_comment")]
    public string? RejectComment { get; set; }

    private string? _callbackUrl;
    [JsonProperty("callback_url")]
    public string? CallbackUrl { get => _callbackUrl; set => _callbackUrl = MarkDirty("callbackUrl", value); }

    private CallbackMethod? _callbackMethod;
    [JsonProperty("callback_method")]
    public CallbackMethod? CallbackMethod { get => _callbackMethod; set => _callbackMethod = MarkDirty("callbackMethod", value); }

    private string? _externalReferenceId;
    [JsonProperty("external_reference_id")]
    public string? ExternalReferenceId { get => _externalReferenceId; set => _externalReferenceId = MarkDirty("externalReferenceId", value); }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    private Address? _address;
    [JsonProperty("address")]
    public Address? Address { get => _address; set => _address = MarkDirty("address", value); }

    private EmergencyCallingService? _emergencyCallingService;
    [JsonProperty("emergency_calling_service")]
    public EmergencyCallingService? EmergencyCallingService
    {
        get => _emergencyCallingService;
        set => _emergencyCallingService = MarkDirty("emergencyCallingService", value);
    }

    private List<Did>? _dids;
    [JsonProperty("dids")]
    public List<Did>? Dids { get => _dids; set => _dids = MarkDirty("dids", value); }
}
