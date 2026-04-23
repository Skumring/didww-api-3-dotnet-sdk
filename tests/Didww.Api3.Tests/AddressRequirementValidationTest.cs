using Didww.Api3.Exception;
using Didww.Api3.Resource;
using FluentAssertions;

namespace Didww.Api3.Tests;

public class AddressRequirementValidationTest : BaseTest
{
    [Fact]
    public async Task TestCreateAddressRequirementValidation()
    {
        StubPost("address_requirement_validations", "address_requirement_validations/create_request.json", "address_requirement_validations/create.json");

        var validation = new AddressRequirementValidation
        {
            AddressRequirement = new AddressRequirement { Id = "aea92b24-a044-4864-9740-89d3e15b65c7" },
            Address = Address.Build("d3414687-40f4-4346-a267-c2c65117d28c")
        };

        var response = await Client.AddressRequirementValidations().CreateAsync(validation);
        var created = response.Data;

        created.Id.Should().Be("aea92b24-a044-4864-9740-89d3e15b65c7");
    }

    [Fact]
    public async Task TestCreateAddressRequirementValidationFailed()
    {
        StubPost("address_requirement_validations", "address_requirement_validations/create_1.json", 422);

        var validation = new AddressRequirementValidation
        {
            AddressRequirement = new AddressRequirement { Id = "aea92b24-a044-4864-9740-89d3e15b65c7" },
            Address = Address.Build("d3414687-40f4-4346-a267-c2c65117d28c")
        };

        var act = async () => await Client.AddressRequirementValidations().CreateAsync(validation);

        var ex = await act.Should().ThrowAsync<DidwwApiException>();
        ex.Which.HttpStatus.Should().Be(422);
        ex.Which.Errors.Should().NotBeEmpty();
    }
}
