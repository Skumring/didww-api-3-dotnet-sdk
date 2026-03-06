using Didww.Api3.Resource.Enums;
using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public interface IProofEntity { }

public class Identity : BaseResource, IProofEntity
{
    public override string Type => "identities";

    public static Identity Build(string id) => BaseResource.Build<Identity>(id);

    private string? _firstName;
    [JsonProperty("first_name")]
    public string? FirstName { get => _firstName; set => _firstName = MarkDirty("firstName", value); }

    private string? _lastName;
    [JsonProperty("last_name")]
    public string? LastName { get => _lastName; set => _lastName = MarkDirty("lastName", value); }

    private string? _phoneNumber;
    [JsonProperty("phone_number")]
    public string? PhoneNumber { get => _phoneNumber; set => _phoneNumber = MarkDirty("phoneNumber", value); }

    private string? _idNumber;
    [JsonProperty("id_number")]
    public string? IdNumber { get => _idNumber; set => _idNumber = MarkDirty("idNumber", value); }

    private string? _birthDate;
    [JsonProperty("birth_date")]
    public string? BirthDate { get => _birthDate; set => _birthDate = MarkDirty("birthDate", value); }

    private string? _companyName;
    [JsonProperty("company_name")]
    public string? CompanyName { get => _companyName; set => _companyName = MarkDirty("companyName", value); }

    private string? _companyRegNumber;
    [JsonProperty("company_reg_number")]
    public string? CompanyRegNumber { get => _companyRegNumber; set => _companyRegNumber = MarkDirty("companyRegNumber", value); }

    private string? _vatId;
    [JsonProperty("vat_id")]
    public string? VatId { get => _vatId; set => _vatId = MarkDirty("vatId", value); }

    private string? _description;
    [JsonProperty("description")]
    public string? Description { get => _description; set => _description = MarkDirty("description", value); }

    private string? _personalTaxId;
    [JsonProperty("personal_tax_id")]
    public string? PersonalTaxId { get => _personalTaxId; set => _personalTaxId = MarkDirty("personalTaxId", value); }

    private IdentityType? _identityType;
    [JsonProperty("identity_type")]
    public IdentityType? IdentityType { get => _identityType; set => _identityType = MarkDirty("identityType", value); }

    private string? _externalReferenceId;
    [JsonProperty("external_reference_id")]
    public string? ExternalReferenceId { get => _externalReferenceId; set => _externalReferenceId = MarkDirty("externalReferenceId", value); }

    private string? _contactEmail;
    [JsonProperty("contact_email")]
    public string? ContactEmail { get => _contactEmail; set => _contactEmail = MarkDirty("contactEmail", value); }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("verified")]
    public bool? Verified { get; set; }

    private Country? _country;
    [JsonProperty("country")]
    public Country? Country { get => _country; set => _country = MarkDirty("country", value); }

    [JsonProperty("proofs")]
    public List<Proof>? Proofs { get; set; }

    [JsonProperty("addresses")]
    public List<Address>? Addresses { get; set; }

    [JsonProperty("permanent_documents")]
    public List<PermanentSupportingDocument>? PermanentDocuments { get; set; }
}
