using Didww.Api3.Resource;
using Didww.Api3.Resource.Enums;
using FluentAssertions;

namespace Didww.Api3.Tests;

public class EmergencyVerificationTest : BaseTest
{
    [Fact]
    public async Task TestListEmergencyVerifications()
    {
        StubGet("emergency_verifications", "emergency_verifications/index.json");

        var response = await Client.EmergencyVerifications().ListAsync();
        var verifications = response.Data;

        verifications.Should().NotBeEmpty();
        verifications.Should().HaveCount(1);

        var first = verifications[0];
        first.Id.Should().Be("11111111-2222-3333-4444-555555555555");
        first.Reference.Should().Be("EV-0001");
        first.Status.Should().Be(EmergencyVerificationStatus.Pending);
        first.CallbackUrl.Should().Be("https://example.com/emergency/hook");
        first.CallbackMethod.Should().Be(CallbackMethod.Post);
        first.ExternalReferenceId.Should().BeNull();
        first.CreatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task TestFindEmergencyVerification()
    {
        StubGet("emergency_verifications/01234567-89ab-cdef-0123-456789abcdef",
            "emergency_verifications/show.json");

        var response = await Client.EmergencyVerifications()
            .FindAsync("01234567-89ab-cdef-0123-456789abcdef");
        var verification = response.Data;

        verification.Id.Should().Be("01234567-89ab-cdef-0123-456789abcdef");
        verification.Reference.Should().Be("EV-0042");
        verification.Status.Should().Be(EmergencyVerificationStatus.Rejected);
        verification.RejectReasons.Should().BeEquivalentTo(new[]
        {
            "Address does not match identity",
            "Missing proof of occupancy"
        });
        verification.RejectComment.Should().Be("Please re-submit with updated documentation.");
        verification.ExternalReferenceId.Should().Be("ref-xyz-999");
    }

    [Fact]
    public async Task TestCreateEmergencyVerification()
    {
        StubPost("emergency_verifications",
            "emergency_verifications/create_request.json",
            "emergency_verifications/create.json");

        var verification = new EmergencyVerification
        {
            CallbackUrl = "https://example.com/emergency/hook",
            CallbackMethod = CallbackMethod.Post,
            ExternalReferenceId = "ref-abc-123",
            EmergencyCallingService = EmergencyCallingService.Build("33333333-4444-5555-6666-777777777777"),
            Address = Address.Build("88888888-9999-aaaa-bbbb-cccccccccccc"),
            Dids = new List<Did> { Did.Build("11111111-aaaa-bbbb-cccc-dddddddddddd") }
        };

        var response = await Client.EmergencyVerifications().CreateAsync(verification);
        var created = response.Data;

        created.Id.Should().Be("bbbbbbbb-cccc-dddd-eeee-ffffffffffff");
        created.Status.Should().Be(EmergencyVerificationStatus.Pending);
        created.ExternalReferenceId.Should().Be("ref-abc-123");
    }

    [Fact]
    public async Task TestUpdateEmergencyVerificationExternalReferenceId()
    {
        StubPatch("emergency_verifications/01234567-89ab-cdef-0123-456789abcdef",
            "emergency_verifications/update_external_reference_id_request.json",
            "emergency_verifications/update_external_reference_id.json");

        var verification = EmergencyVerification.Build("01234567-89ab-cdef-0123-456789abcdef");
        verification.ExternalReferenceId = "updated-ev-ref-77";

        var response = await Client.EmergencyVerifications().UpdateAsync(verification);
        var updated = response.Data;

        updated.Id.Should().Be("01234567-89ab-cdef-0123-456789abcdef");
        updated.ExternalReferenceId.Should().Be("updated-ev-ref-77");
    }
}
