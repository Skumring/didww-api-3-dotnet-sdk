using Didww.Api3.Resource.Enums;
using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class VoiceOutTrunk : BaseResource
{
    public override string Type => "voice_out_trunks";

    public static VoiceOutTrunk Build(string id) => BaseResource.Build<VoiceOutTrunk>(id);

    private string? _name;
    [JsonProperty("name")]
    public string? Name { get => _name; set => _name = MarkDirty("name", value); }

    [JsonProperty("allowed_sip_ips")]
    public List<string>? AllowedSipIps { get; set; }

    private OnCliMismatchAction? _onCliMismatchAction;
    [JsonProperty("on_cli_mismatch_action")]
    public OnCliMismatchAction? OnCliMismatchAction { get => _onCliMismatchAction; set => _onCliMismatchAction = MarkDirty("onCliMismatchAction", value); }

    [JsonProperty("allowed_rtp_ips")]
    public List<string>? AllowedRtpIps { get; set; }

    [JsonProperty("allow_any_did_as_cli")]
    public bool? AllowAnyDidAsCli { get; set; }

    [JsonProperty("status")]
    public VoiceOutTrunkStatus? Status { get; set; }

    private int? _capacityLimit;
    [JsonProperty("capacity_limit")]
    public int? CapacityLimit { get => _capacityLimit; set => _capacityLimit = MarkDirty("capacityLimit", value); }

    [JsonProperty("threshold_amount")]
    public double? ThresholdAmount { get; set; }

    private MediaEncryptionMode? _mediaEncryptionMode;
    [JsonProperty("media_encryption_mode")]
    public MediaEncryptionMode? MediaEncryptionMode { get => _mediaEncryptionMode; set => _mediaEncryptionMode = MarkDirty("mediaEncryptionMode", value); }

    private DefaultDstAction? _defaultDstAction;
    [JsonProperty("default_dst_action")]
    public DefaultDstAction? DefaultDstAction { get => _defaultDstAction; set => _defaultDstAction = MarkDirty("defaultDstAction", value); }

    [JsonProperty("dst_prefixes")]
    public List<string>? DstPrefixes { get; set; }

    [JsonProperty("force_symmetric_rtp")]
    public bool? ForceSymmetricRtp { get; set; }

    [JsonProperty("rtp_ping")]
    public bool? RtpPing { get; set; }

    [JsonProperty("callback_url")]
    public string? CallbackUrl { get; set; }

    [JsonProperty("username")]
    public string? Username { get; set; }

    [JsonProperty("password")]
    public string? Password { get; set; }

    [JsonProperty("threshold_reached")]
    public bool? ThresholdReached { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("dids")]
    public List<Did>? Dids { get; set; }

    [JsonProperty("default_did")]
    public Did? DefaultDid { get; set; }

    [JsonProperty("voice_in_trunk_group")]
    public VoiceInTrunkGroup? VoiceInTrunkGroup { get; set; }
}
