using FluentAssertions;

namespace Didww.Api3.Tests;

public class EmergencyRequirementTest : BaseTest
{
    [Fact]
    public async Task TestListEmergencyRequirements()
    {
        StubGet("emergency_requirements", "emergency_requirements/index.json");

        var response = await Client.EmergencyRequirements().ListAsync();
        var requirements = response.Data;

        requirements.Should().NotBeEmpty();
        requirements.Should().HaveCount(1);

        var first = requirements[0];
        first.Id.Should().Be("11111111-2222-3333-4444-555555555555");
        first.IdentityType.Should().Be("personal");
        first.AddressAreaLevel.Should().Be("city");
        first.PersonalAreaLevel.Should().Be("city");
        first.BusinessAreaLevel.Should().Be("city");
        first.AddressMandatoryFields.Should().BeEquivalentTo(new[] { "street", "city", "postal_code" });
        first.PersonalMandatoryFields.Should().BeEquivalentTo(new[] { "first_name", "last_name" });
        first.BusinessMandatoryFields.Should().BeEquivalentTo(new[] { "company_name", "tax_number" });
        first.EstimateSetupTime.Should().Be("3");
        first.RequirementRestrictionMessage.Should().BeNull();
    }

    [Fact]
    public async Task TestFindEmergencyRequirement()
    {
        StubGet("emergency_requirements/01234567-89ab-cdef-0123-456789abcdef",
            "emergency_requirements/show.json");

        var response = await Client.EmergencyRequirements()
            .FindAsync("01234567-89ab-cdef-0123-456789abcdef");
        var requirement = response.Data;

        requirement.Id.Should().Be("01234567-89ab-cdef-0123-456789abcdef");
        requirement.IdentityType.Should().Be("business");
        requirement.EstimateSetupTime.Should().Be("5");
        requirement.RequirementRestrictionMessage.Should()
            .Be("Additional compliance review is required for this country.");
    }
}
