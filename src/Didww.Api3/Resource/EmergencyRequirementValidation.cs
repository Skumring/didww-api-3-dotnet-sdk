using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class EmergencyRequirementValidation : BaseResource
{
    public override string Type => "emergency_requirement_validations";
    public static EmergencyRequirementValidation Build(string id) => BaseResource.Build<EmergencyRequirementValidation>(id);

    [JsonProperty("result")] public bool? Result { get; set; }
    [JsonProperty("errors")] public Dictionary<string, object>? Errors { get; set; }

    private EmergencyRequirement? _emergencyRequirement;
    [JsonProperty("emergency_requirement")]
    public EmergencyRequirement? EmergencyRequirement
    {
        get => _emergencyRequirement;
        set => _emergencyRequirement = MarkDirty("emergencyRequirement", value);
    }

    private Address? _address;
    [JsonProperty("address")]
    public Address? Address { get => _address; set => _address = MarkDirty("address", value); }

    private Identity? _identity;
    [JsonProperty("identity")]
    public Identity? Identity { get => _identity; set => _identity = MarkDirty("identity", value); }
}
