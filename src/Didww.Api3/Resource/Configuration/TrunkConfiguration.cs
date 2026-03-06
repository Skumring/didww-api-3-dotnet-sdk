using Didww.Api3.Resource.Enums;
using Newtonsoft.Json;

namespace Didww.Api3.Resource.Configuration;

public abstract class TrunkConfiguration
{
    [JsonIgnore]
    public abstract string ConfigurationType { get; }
}

public class SipConfiguration : TrunkConfiguration
{
    [JsonIgnore]
    public override string ConfigurationType => "sip_configurations";

    [JsonProperty("username")]
    public string? Username { get; set; }

    [JsonProperty("host")]
    public string? Host { get; set; }

    [JsonProperty("port")]
    public int? Port { get; set; }

    [JsonProperty("codec_ids")]
    public List<Codec>? CodecIds { get; set; }

    [JsonProperty("rx_dtmf_format_id")]
    public RxDtmfFormat? RxDtmfFormatId { get; set; }

    [JsonProperty("tx_dtmf_format_id")]
    public TxDtmfFormat? TxDtmfFormatId { get; set; }

    [JsonProperty("resolve_ruri")]
    public bool? ResolveRuri { get; set; }

    [JsonProperty("auth_enabled")]
    public bool? AuthEnabled { get; set; }

    [JsonProperty("auth_user")]
    public string? AuthUser { get; set; }

    [JsonProperty("auth_password")]
    public string? AuthPassword { get; set; }

    [JsonProperty("auth_from_user")]
    public string? AuthFromUser { get; set; }

    [JsonProperty("auth_from_domain")]
    public string? AuthFromDomain { get; set; }

    [JsonProperty("sst_enabled")]
    public bool? SstEnabled { get; set; }

    [JsonProperty("sst_min_timer")]
    public int? SstMinTimer { get; set; }

    [JsonProperty("sst_max_timer")]
    public int? SstMaxTimer { get; set; }

    [JsonProperty("sst_accept_501")]
    public bool? SstAccept501 { get; set; }

    [JsonProperty("sst_session_expires")]
    public int? SstSessionExpires { get; set; }

    [JsonProperty("sst_refresh_method_id")]
    public SstRefreshMethod? SstRefreshMethodId { get; set; }

    [JsonProperty("sip_timer_b")]
    public int? SipTimerB { get; set; }

    [JsonProperty("dns_srv_failover_timer")]
    public int? DnsSrvFailoverTimer { get; set; }

    [JsonProperty("rtp_ping")]
    public bool? RtpPing { get; set; }

    [JsonProperty("force_symmetric_rtp")]
    public bool? ForceSymmetricRtp { get; set; }

    [JsonProperty("rerouting_disconnect_code_ids")]
    public List<ReroutingDisconnectCode>? ReroutingDisconnectCodeIds { get; set; }

    [JsonProperty("transport_protocol_id")]
    public TransportProtocol? TransportProtocolId { get; set; }

    [JsonProperty("media_encryption_mode")]
    public MediaEncryptionMode? MediaEncryptionMode { get; set; }

    [JsonProperty("stir_shaken_mode")]
    public StirShakenMode? StirShakenMode { get; set; }

    [JsonProperty("max_transfers")]
    public int? MaxTransfers { get; set; }

    [JsonProperty("max_30x_redirects")]
    public int? Max30xRedirects { get; set; }

    [JsonProperty("allowed_rtp_ips")]
    public List<string>? AllowedRtpIps { get; set; }
}

public class PstnConfiguration : TrunkConfiguration
{
    [JsonIgnore]
    public override string ConfigurationType => "pstn_configurations";

    [JsonProperty("dst")]
    public string? Dst { get; set; }
}
