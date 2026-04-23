using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class AddressRequirementValidation : BaseResource
{
    public override string Type => "address_requirement_validations";
    public static AddressRequirementValidation Build(string id) => BaseResource.Build<AddressRequirementValidation>(id);

    [JsonProperty("result")] public bool? Result { get; set; }
    [JsonProperty("errors")] public Dictionary<string, object>? Errors { get; set; }

    private AddressRequirement? _addressRequirement;
    [JsonProperty("address_requirement")]
    public AddressRequirement? AddressRequirement { get => _addressRequirement; set => _addressRequirement = MarkDirty("addressRequirement", value); }

    private Address? _address;
    [JsonProperty("address")]
    public Address? Address { get => _address; set => _address = MarkDirty("address", value); }

    private Identity? _identity;
    [JsonProperty("identity")]
    public Identity? Identity { get => _identity; set => _identity = MarkDirty("identity", value); }
}
