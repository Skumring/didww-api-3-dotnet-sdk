using Didww.Api3.Http;
using Didww.Api3.Resource;
using Didww.Api3.Resource.Enums;
using FluentAssertions;

namespace Didww.Api3.Tests;

public class RegionTest : BaseTest
{
    [Fact]
    public async Task TestListRegions()
    {
        StubGet("regions", "regions/index.json");
        var response = await Client.Regions().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}

public class CityTest : BaseTest
{
    [Fact]
    public async Task TestListCities()
    {
        StubGet("cities", "cities/index.json");
        var response = await Client.Cities().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}

public class AreaTest : BaseTest
{
    [Fact]
    public async Task TestListAreas()
    {
        StubGet("areas", "areas/index.json");
        var response = await Client.Areas().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}

public class PopTest : BaseTest
{
    [Fact]
    public async Task TestListPops()
    {
        StubGet("pops", "pops/index.json");
        var response = await Client.Pops().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}

public class DidGroupTypeTest : BaseTest
{
    [Fact]
    public async Task TestListDidGroupTypes()
    {
        StubGet("did_group_types", "did_group_types/index.json");
        var response = await Client.DidGroupTypes().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}

public class DidGroupTest : BaseTest
{
    [Fact]
    public async Task TestListDidGroups()
    {
        StubGet("did_groups", "did_groups/index.json");
        var response = await Client.DidGroups().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}

public class AvailableDidTest : BaseTest
{
    [Fact]
    public async Task TestListAvailableDids()
    {
        StubGet("available_dids", "available_dids/index.json");
        var response = await Client.AvailableDids().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}

public class NanpaPrefixTest : BaseTest
{
    [Fact]
    public async Task TestListNanpaPrefixes()
    {
        StubGet("nanpa_prefixes", "nanpa_prefixes/index.json");
        var response = await Client.NanpaPrefixes().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}

public class CapacityPoolTest : BaseTest
{
    [Fact]
    public async Task TestListCapacityPools()
    {
        StubGet("capacity_pools", "capacity_pools/index.json");
        var response = await Client.CapacityPools().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}

public class SharedCapacityGroupTest : BaseTest
{
    [Fact]
    public async Task TestListSharedCapacityGroups()
    {
        StubGet("shared_capacity_groups", "shared_capacity_groups/index.json");
        var response = await Client.SharedCapacityGroups().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}

public class DidReservationTest : BaseTest
{
    [Fact]
    public async Task TestListDidReservations()
    {
        StubGet("did_reservations", "did_reservations/index.json");
        var response = await Client.DidReservations().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}

public class ExportTest : BaseTest
{
    [Fact]
    public async Task TestListExports()
    {
        StubGet("exports", "exports/index.json");
        var response = await Client.Exports().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}

public class IdentityTest : BaseTest
{
    [Fact]
    public async Task TestListIdentities()
    {
        StubGet("identities", "identities/index.json");
        var response = await Client.Identities().ListAsync();
        response.Data.Should().NotBeEmpty();
    }

}

public class AddressTest : BaseTest
{
    [Fact]
    public async Task TestListAddresses()
    {
        StubGet("addresses", "addresses/index.json");
        var response = await Client.Addresses().ListAsync();
        response.Data.Should().NotBeEmpty();
    }

}

public class AddressVerificationTest : BaseTest
{
    [Fact]
    public async Task TestListAddressVerifications()
    {
        StubGet("address_verifications", "address_verifications/index.json");
        var response = await Client.AddressVerifications().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}

public class ProofTypeTest : BaseTest
{
    [Fact]
    public async Task TestListProofTypes()
    {
        StubGet("proof_types", "proof_types/index.json");
        var response = await Client.ProofTypes().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}

public class PublicKeyTest : BaseTest
{
    [Fact]
    public async Task TestListPublicKeys()
    {
        StubGet("public_keys", "public_keys/index.json");
        var response = await Client.PublicKeys().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}

public class RequirementTest : BaseTest
{
    [Fact]
    public async Task TestListRequirements()
    {
        StubGet("requirements", "requirements/index.json");
        var response = await Client.Requirements().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}

public class SupportingDocumentTemplateTest : BaseTest
{
    [Fact]
    public async Task TestListSupportingDocumentTemplates()
    {
        StubGet("supporting_document_templates", "supporting_document_templates/index.json");
        var response = await Client.SupportingDocumentTemplates().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}



public class RequirementValidationTest : BaseTest
{
    [Fact]
    public async Task TestCreateRequirementValidation()
    {
        StubPost("requirement_validations", "requirement_validations/create.json");

        var validation = new RequirementValidation();
        validation.Address = Address.Build("d3414687-40f4-4346-a267-c2c65117d28c");
        validation.Requirement = new Requirement { Id = "aea92b24-a044-4864-9740-89d3e15b65c7" };

        var response = await Client.RequirementValidations().CreateAsync(validation);
        response.Data.Id.Should().Be("aea92b24-a044-4864-9740-89d3e15b65c7");
    }
}

public class PermanentSupportingDocumentTest : BaseTest
{
    [Fact]
    public async Task TestCreatePermanentSupportingDocument()
    {
        StubPost("permanent_supporting_documents", "permanent_supporting_documents/create.json");

        var doc = new PermanentSupportingDocument();
        doc.Identity = Identity.Build("5e9df058-50d2-4e34-b0d4-d1746b86f41a");
        doc.Template = SupportingDocumentTemplate.Build("4199435f-646e-4e9d-a143-8f3b972b10c5");
        doc.Files = new List<EncryptedFile> { EncryptedFile.Build("254b3c2d-c40c-4ff7-93b1-a677aee7fa10") };

        var queryParams = new QueryParams().Include("template");
        var response = await Client.PermanentSupportingDocuments().CreateAsync(doc, queryParams);
        var created = response.Data;

        created.Id.Should().Be("19510da3-c07e-4fa9-a696-6b9ab89cc172");
        created.CreatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task TestDeletePermanentSupportingDocument()
    {
        var id = "19510da3-c07e-4fa9-a696-6b9ab89cc172";
        StubDelete("permanent_supporting_documents/" + id);
        await Client.PermanentSupportingDocuments().DeleteAsync(id);
    }
}

public class EncryptedFileTest : BaseTest
{
    [Fact]
    public async Task TestListEncryptedFiles()
    {
        StubGet("encrypted_files", "encrypted_files/index.json");
        var response = await Client.EncryptedFiles().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}

public class VoiceOutTrunkTest : BaseTest
{
    [Fact]
    public async Task TestListVoiceOutTrunks()
    {
        StubGet("voice_out_trunks", "voice_out_trunks/index.json");
        var response = await Client.VoiceOutTrunks().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}

public class VoiceInTrunkGroupTest : BaseTest
{
    [Fact]
    public async Task TestListVoiceInTrunkGroups()
    {
        StubGet("voice_in_trunk_groups", "voice_in_trunk_groups/index.json");
        var response = await Client.VoiceInTrunkGroups().ListAsync();
        response.Data.Should().NotBeEmpty();
    }
}
