using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class VoiceInTrunkGroup : BaseResource
{
    public override string Type => "voice_in_trunk_groups";

    public static VoiceInTrunkGroup Build(string id) => BaseResource.Build<VoiceInTrunkGroup>(id);

    private string? _name;
    [JsonProperty("name")]
    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private int? _capacityLimit;
    [JsonProperty("capacity_limit")]
    public int? CapacityLimit
    {
        get => _capacityLimit;
        set => SetProperty(ref _capacityLimit, value);
    }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    private List<VoiceInTrunk>? _voiceInTrunks;
    [JsonProperty("voice_in_trunks")]
    public List<VoiceInTrunk>? VoiceInTrunks
    {
        get => _voiceInTrunks;
        set => SetProperty(ref _voiceInTrunks, value);
    }
}
