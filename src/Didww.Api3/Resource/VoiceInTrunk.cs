using Didww.Api3.Converter;
using Didww.Api3.Resource.Configuration;
using Didww.Api3.Resource.Enums;
using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class VoiceInTrunk : BaseResource
{
    public override string Type => "voice_in_trunks";

    public static VoiceInTrunk Build(string id) => BaseResource.Build<VoiceInTrunk>(id);

    private string? _name;
    [JsonProperty("name")]
    public string? Name
    {
        get => _name;
        set => _name = MarkDirty("name", value);
    }

    private int? _priority;
    [JsonProperty("priority")]
    public int? Priority
    {
        get => _priority;
        set => _priority = MarkDirty("priority", value);
    }

    private int? _weight;
    [JsonProperty("weight")]
    public int? Weight
    {
        get => _weight;
        set => _weight = MarkDirty("weight", value);
    }

    private CliFormat? _cliFormat;
    [JsonProperty("cli_format")]
    public CliFormat? CliFormat
    {
        get => _cliFormat;
        set => _cliFormat = MarkDirty("cliFormat", value);
    }

    private string? _cliPrefix;
    [JsonProperty("cli_prefix")]
    public string? CliPrefix
    {
        get => _cliPrefix;
        set => _cliPrefix = MarkDirty("cliPrefix", value);
    }

    private string? _description;
    [JsonProperty("description")]
    public string? Description
    {
        get => _description;
        set => _description = MarkDirty("description", value);
    }

    private int? _ringingTimeout;
    [JsonProperty("ringing_timeout")]
    public int? RingingTimeout
    {
        get => _ringingTimeout;
        set => _ringingTimeout = MarkDirty("ringingTimeout", value);
    }

    private int? _capacityLimit;
    [JsonProperty("capacity_limit")]
    public int? CapacityLimit
    {
        get => _capacityLimit;
        set => _capacityLimit = MarkDirty("capacityLimit", value);
    }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    private TrunkConfiguration? _configuration;
    [JsonProperty("configuration")]
    [JsonConverter(typeof(TrunkConfigurationConverter))]
    public TrunkConfiguration? Configuration
    {
        get => _configuration;
        set => _configuration = MarkDirty("configuration", value);
    }

    private Pop? _pop;
    [JsonProperty("pop")]
    public Pop? Pop
    {
        get => _pop;
        set => _pop = MarkDirty("pop", value);
    }

    private VoiceInTrunkGroup? _voiceInTrunkGroup;
    [JsonProperty("voice_in_trunk_group")]
    public VoiceInTrunkGroup? VoiceInTrunkGroup
    {
        get => _voiceInTrunkGroup;
        set => _voiceInTrunkGroup = MarkDirty("voiceInTrunkGroup", value);
    }
}
