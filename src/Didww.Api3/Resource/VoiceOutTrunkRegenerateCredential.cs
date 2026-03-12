using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class VoiceOutTrunkRegenerateCredential : BaseResource
{
    public override string Type => "voice_out_trunk_regenerate_credentials";

    public static VoiceOutTrunkRegenerateCredential Build(string id) => BaseResource.Build<VoiceOutTrunkRegenerateCredential>(id);

    [JsonProperty("username")]
    public string? Username { get; set; }

    [JsonProperty("password")]
    public string? Password { get; set; }

    private VoiceOutTrunk? _voiceOutTrunk;
    [JsonProperty("voice_out_trunk")]
    public VoiceOutTrunk? VoiceOutTrunk
    {
        get => _voiceOutTrunk;
        set => SetProperty(ref _voiceOutTrunk, value);
    }
}
