using System.Runtime.Serialization;
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

    // Suppresses the auto-cascade in property setters during JSON
    // deserialization. The server's response shape is already consistent —
    // running the cascade against it would clobber valid combinations
    // (e.g. EnabledSipRegistration: true would null out a Host the server
    // had set before the deserializer reached EnabledSipRegistration).
    // The cascade logic only fires when the application sets a property.
    [JsonIgnore]
    private bool _isDeserializing;

    [OnDeserializing]
    internal void OnDeserializing(StreamingContext _) => _isDeserializing = true;

    [OnDeserialized]
    internal void OnDeserialized(StreamingContext _) => _isDeserializing = false;

    [JsonProperty("username")]
    public string? Username { get; set; }

    private string? _host;
    private int? _port;
    private bool _hostExplicit;
    private bool _portExplicit;

    /// <summary>
    /// Setting <see cref="Host"/> to a non-null value cascades
    /// <see cref="EnabledSipRegistration"/> = false and
    /// <see cref="UseDidInRuri"/> = false because the server requires those
    /// states whenever <c>host</c> is present (API 2026-04-16). The wire
    /// payload reflects the cascaded state.
    /// </summary>
    [JsonProperty("host", NullValueHandling = NullValueHandling.Include)]
    public string? Host
    {
        get => _host;
        set
        {
            if (!_isDeserializing)
            {
                _hostExplicit = true;
                if (value != null)
                {
                    _enabledSipRegistration = false;
                    UseDidInRuri = false;
                }
            }
            _host = value;
        }
    }

    [JsonProperty("port", NullValueHandling = NullValueHandling.Include)]
    public int? Port
    {
        get => _port;
        set
        {
            if (!_isDeserializing)
                _portExplicit = true;
            _port = value;
        }
    }

    public bool ShouldSerializeHost() => _hostExplicit;
    public bool ShouldSerializePort() => _portExplicit;

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

    [JsonProperty("diversion_relay_policy")]
    public DiversionRelayPolicy? DiversionRelayPolicy { get; set; }

    [JsonProperty("diversion_inject_mode")]
    public DiversionInjectMode? DiversionInjectMode { get; set; }

    [JsonProperty("network_protocol_priority")]
    public NetworkProtocolPriority? NetworkProtocolPriority { get; set; }

    /// <summary>
    /// Whether SIP registration is enabled. When <c>true</c> the server
    /// generates <see cref="IncomingAuthUsername"/> /
    /// <see cref="IncomingAuthPassword"/> and the trunk's
    /// <see cref="Host"/> and <see cref="Port"/> must be left blank. When
    /// disabling sip registration on an existing trunk, the same PATCH must
    /// also set <see cref="Host"/> to a non-blank value and
    /// <see cref="UseDidInRuri"/> to <c>false</c>, or the server returns 422.
    /// (API 2026-04-16)
    /// </summary>
    private bool? _enabledSipRegistration;

    /// <summary>
    /// Setting <see cref="EnabledSipRegistration"/> cascades dependent fields
    /// to satisfy the server's validation rules (API 2026-04-16):
    /// <list type="bullet">
    /// <item>
    /// <c>true</c> -> nullify <see cref="Host"/> / <see cref="Port"/> and
    /// emit them as <c>null</c> on the wire (host/port must be blank when
    /// sip_registration is on). The wire emission fires unconditionally so
    /// PATCH against an existing trunk that already has host/port set
    /// server-side is told to clear them.
    /// </item>
    /// <item>
    /// <c>false</c> -> force <see cref="UseDidInRuri"/> = false (must be
    /// disabled when sip_registration is disabled).
    /// </item>
    /// </list>
    /// </summary>
    [JsonProperty("enabled_sip_registration")]
    public bool? EnabledSipRegistration
    {
        get => _enabledSipRegistration;
        set
        {
            if (!_isDeserializing)
            {
                if (value == true)
                {
                    // Always emit host: null and port: null on the wire so a
                    // PATCH against an existing trunk that already has them
                    // persisted on the server side is told to clear them.
                    _host = null;
                    _hostExplicit = true;
                    _port = null;
                    _portExplicit = true;
                }
                else if (value == false)
                {
                    UseDidInRuri = false;
                }
            }
            _enabledSipRegistration = value;
        }
    }

    [JsonProperty("use_did_in_ruri")]
    public bool? UseDidInRuri { get; set; }

    [JsonProperty("cnam_lookup")]
    public bool? CnamLookup { get; set; }

    /// <summary>
    /// Server-generated SIP authentication username, returned in responses
    /// when <see cref="EnabledSipRegistration"/> is <c>true</c>.
    /// Read-only: the API rejects any write attempt with HTTP 400 "Param not allowed".
    /// (API 2026-04-16)
    /// </summary>
    [JsonProperty("incoming_auth_username")]
    public string? IncomingAuthUsername { get; private set; }

    /// <summary>
    /// Server-generated SIP authentication password, returned in responses
    /// when <see cref="EnabledSipRegistration"/> is <c>true</c>.
    /// Read-only: the API rejects any write attempt with HTTP 400 "Param not allowed".
    /// (API 2026-04-16)
    /// </summary>
    [JsonProperty("incoming_auth_password")]
    public string? IncomingAuthPassword { get; private set; }

    // Read-only fields: deserialize from server responses but never serialize
    // back into POST/PATCH request bodies. Newtonsoft.Json discovers these
    // ShouldSerialize* methods via reflection and skips the property when
    // they return false.
    public bool ShouldSerializeIncomingAuthUsername() => false;
    public bool ShouldSerializeIncomingAuthPassword() => false;

    // Override ToString so default logging / debugger / error reports never
    // leak SIP credentials. The wire payload is unaffected — Newtonsoft.Json
    // serializes the real values (or strips read-only ones via the
    // ShouldSerialize* hooks above).
    public override string ToString()
    {
        static string Mask(string? v) => string.IsNullOrEmpty(v) ? "null" : "[FILTERED]";
        return $"SipConfiguration(Username={Username ?? "null"}, Host={Host ?? "null"}, Port={Port?.ToString() ?? "null"}, " +
               $"AuthPassword={Mask(AuthPassword)}, EnabledSipRegistration={EnabledSipRegistration?.ToString() ?? "null"}, " +
               $"IncomingAuthUsername={Mask(IncomingAuthUsername)}, IncomingAuthPassword={Mask(IncomingAuthPassword)})";
    }
}

public class PstnConfiguration : TrunkConfiguration
{
    [JsonIgnore]
    public override string ConfigurationType => "pstn_configurations";

    [JsonProperty("dst")]
    public string? Dst { get; set; }
}
