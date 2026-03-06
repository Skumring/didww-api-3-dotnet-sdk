using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class Address : BaseResource, IProofEntity
{
    public override string Type => "addresses";

    public static Address Build(string id) => BaseResource.Build<Address>(id);

    private string? _cityName;
    [JsonProperty("city_name")]
    public string? CityName { get => _cityName; set => _cityName = MarkDirty("cityName", value); }

    private string? _postalCode;
    [JsonProperty("postal_code")]
    public string? PostalCode { get => _postalCode; set => _postalCode = MarkDirty("postalCode", value); }

    private string? _address;
    [JsonProperty("address")]
    public string? AddressLine { get => _address; set => _address = MarkDirty("address", value); }

    private string? _description;
    [JsonProperty("description")]
    public string? Description { get => _description; set => _description = MarkDirty("description", value); }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("verified")]
    public bool? Verified { get; set; }

    private Country? _country;
    [JsonProperty("country")]
    public Country? Country { get => _country; set => _country = MarkDirty("country", value); }

    private Identity? _identity;
    [JsonProperty("identity")]
    public Identity? Identity { get => _identity; set => _identity = MarkDirty("identity", value); }

    [JsonProperty("proofs")]
    public List<Proof>? Proofs { get; set; }

    private Area? _area;
    [JsonProperty("area")]
    public Area? Area { get => _area; set => _area = MarkDirty("area", value); }

    private City? _city;
    [JsonProperty("city")]
    public City? City { get => _city; set => _city = MarkDirty("city", value); }
}
