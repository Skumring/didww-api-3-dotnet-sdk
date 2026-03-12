using Didww.Api3.Resource.Enums;
using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class VoiceOutTrunk : BaseResource
{
    public override string Type => "voice_out_trunks";

    public static VoiceOutTrunk Build(string id) => BaseResource.Build<VoiceOutTrunk>(id);

    private string? _name;
    [JsonProperty("name")]
    public string? Name { get => _name; set => SetProperty(ref _name, value); }

    private List<string>? _allowedSipIps;
    [JsonProperty("allowed_sip_ips")]
    public List<string>? AllowedSipIps { get => _allowedSipIps; set => SetProperty(ref _allowedSipIps, value); }

    private OnCliMismatchAction? _onCliMismatchAction;
    [JsonProperty("on_cli_mismatch_action")]
    public OnCliMismatchAction? OnCliMismatchAction { get => _onCliMismatchAction; set => SetProperty(ref _onCliMismatchAction, value); }

    private List<string>? _allowedRtpIps;
    [JsonProperty("allowed_rtp_ips")]
    public List<string>? AllowedRtpIps { get => _allowedRtpIps; set => SetProperty(ref _allowedRtpIps, value); }

    private bool? _allowAnyDidAsCli;
    [JsonProperty("allow_any_did_as_cli")]
    public bool? AllowAnyDidAsCli { get => _allowAnyDidAsCli; set => SetProperty(ref _allowAnyDidAsCli, value); }

    [JsonProperty("status")]
    public VoiceOutTrunkStatus? Status { get; set; }

    private int? _capacityLimit;
    [JsonProperty("capacity_limit")]
    public int? CapacityLimit { get => _capacityLimit; set => SetProperty(ref _capacityLimit, value); }

    private decimal? _thresholdAmount;
    [JsonProperty("threshold_amount")]
    public decimal? ThresholdAmount { get => _thresholdAmount; set => SetProperty(ref _thresholdAmount, value); }

    private MediaEncryptionMode? _mediaEncryptionMode;
    [JsonProperty("media_encryption_mode")]
    public MediaEncryptionMode? MediaEncryptionMode { get => _mediaEncryptionMode; set => SetProperty(ref _mediaEncryptionMode, value); }

    private DefaultDstAction? _defaultDstAction;
    [JsonProperty("default_dst_action")]
    public DefaultDstAction? DefaultDstAction { get => _defaultDstAction; set => SetProperty(ref _defaultDstAction, value); }

    private List<string>? _dstPrefixes;
    [JsonProperty("dst_prefixes")]
    public List<string>? DstPrefixes { get => _dstPrefixes; set => SetProperty(ref _dstPrefixes, value); }

    private bool? _forceSymmetricRtp;
    [JsonProperty("force_symmetric_rtp")]
    public bool? ForceSymmetricRtp { get => _forceSymmetricRtp; set => SetProperty(ref _forceSymmetricRtp, value); }

    private bool? _rtpPing;
    [JsonProperty("rtp_ping")]
    public bool? RtpPing { get => _rtpPing; set => SetProperty(ref _rtpPing, value); }

    private string? _callbackUrl;
    [JsonProperty("callback_url")]
    public string? CallbackUrl { get => _callbackUrl; set => SetProperty(ref _callbackUrl, value); }

    [JsonProperty("username")]
    public string? Username { get; set; }

    [JsonProperty("password")]
    public string? Password { get; set; }

    [JsonProperty("threshold_reached")]
    public bool? ThresholdReached { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    private List<Did>? _dids;
    [JsonProperty("dids")]
    public List<Did>? Dids { get => _dids; set => SetProperty(ref _dids, value); }

    private Did? _defaultDid;
    [JsonProperty("default_did")]
    public Did? DefaultDid { get => _defaultDid; set => SetProperty(ref _defaultDid, value); }

    [JsonProperty("voice_in_trunk_group")]
    public VoiceInTrunkGroup? VoiceInTrunkGroup { get; set; }
}
