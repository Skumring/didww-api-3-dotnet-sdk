using Didww.Api3.Exception;
using Didww.Api3.Resource;
using FluentAssertions;

namespace Didww.Api3.Tests;

public class EmergencyRequirementValidationTest : BaseTest
{
    [Fact]
    public async Task TestCreateEmergencyRequirementValidation()
    {
        StubPost("emergency_requirement_validations",
            "emergency_requirement_validations/create_request.json",
            "emergency_requirement_validations/create.json");

        var validation = new EmergencyRequirementValidation
        {
            EmergencyRequirement = EmergencyRequirement.Build("11111111-2222-3333-4444-555555555555"),
            Address = Address.Build("66666666-7777-8888-9999-aaaaaaaaaaaa"),
            Identity = Identity.Build("bbbbbbbb-cccc-dddd-eeee-ffffffffffff")
        };

        var response = await Client.EmergencyRequirementValidations().CreateAsync(validation);
        var created = response.Data;

        created.Id.Should().Be("11111111-2222-3333-4444-555555555555");
    }

    [Fact]
    public async Task TestCreateEmergencyRequirementValidationFailed()
    {
        StubPost("emergency_requirement_validations",
            "emergency_requirement_validations/create_1.json", 422);

        var validation = new EmergencyRequirementValidation
        {
            EmergencyRequirement = EmergencyRequirement.Build("11111111-2222-3333-4444-555555555555"),
            Address = Address.Build("66666666-7777-8888-9999-aaaaaaaaaaaa"),
            Identity = Identity.Build("bbbbbbbb-cccc-dddd-eeee-ffffffffffff")
        };

        var act = async () => await Client.EmergencyRequirementValidations().CreateAsync(validation);

        var ex = await act.Should().ThrowAsync<DidwwApiException>();
        ex.Which.HttpStatus.Should().Be(422);
        ex.Which.Errors.Should().NotBeEmpty();
    }
}
