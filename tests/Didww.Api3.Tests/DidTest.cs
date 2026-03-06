using Didww.Api3.Http;
using Didww.Api3.Resource;
using Didww.Api3.Resource.Enums;
using FluentAssertions;

namespace Didww.Api3.Tests;

public class DidTest : BaseTest
{
    [Fact]
    public async Task TestListDids()
    {
        StubGet("dids", "dids/index.json");

        var queryParams = new QueryParams().Include("order");
        var response = await Client.Dids().ListAsync(queryParams);
        var dids = response.Data;

        dids.Should().NotBeEmpty();
        dids[0].Order.Should().NotBeNull();
        dids[0].Order!.Reference.Should().Be("TZO-560180");
    }

    [Fact]
    public async Task TestFindDid()
    {
        StubGet("dids/9df99644-f1a5-4a3c-99a4-559d758eb96b", "dids/show.json");

        var response = await Client.Dids().FindAsync("9df99644-f1a5-4a3c-99a4-559d758eb96b");
        var did = response.Data;

        did.Number.Should().Be("16091609123456797");
        did.Blocked.Should().BeFalse();
        did.CapacityLimit.Should().Be(2);
        did.Description.Should().Be("something");
        did.Terminated.Should().BeFalse();
        did.AwaitingRegistration.Should().BeFalse();
        did.BillingCyclesCount.Should().BeNull();
        did.ChannelsIncludedCount.Should().Be(0);
        did.DedicatedChannelsCount.Should().Be(0);
    }

    [Fact]
    public async Task TestUpdateDid()
    {
        StubPatch("dids/9df99644-f1a5-4a3c-99a4-559d758eb96b", "dids/update.json");

        var did = Did.Build("9df99644-f1a5-4a3c-99a4-559d758eb96b");
        did.CapacityLimit = 2;
        did.Description = "something";

        var response = await Client.Dids().UpdateAsync(did);
        var updated = response.Data;

        updated.Id.Should().Be("9df99644-f1a5-4a3c-99a4-559d758eb96b");
        updated.Number.Should().Be("16091609123456797");
        updated.Blocked.Should().BeFalse();
        updated.CapacityLimit.Should().Be(2);
        updated.Description.Should().Be("something");
        updated.Terminated.Should().BeFalse();
    }

    [Fact]
    public async Task TestUpdateDidTerminated()
    {
        StubPatch("dids/9df99644-f1a5-4a3c-99a4-559d758eb96b", "dids/update_terminated.json");

        var did = Did.Build("9df99644-f1a5-4a3c-99a4-559d758eb96b");
        did.Terminated = true;

        var response = await Client.Dids().UpdateAsync(did);
        var updated = response.Data;

        updated.Id.Should().Be("9df99644-f1a5-4a3c-99a4-559d758eb96b");
        updated.Blocked.Should().BeTrue();
        updated.Terminated.Should().BeTrue();
        updated.BillingCyclesCount.Should().Be(0);
    }

    [Fact]
    public async Task TestUpdateDidFromLoadedResource()
    {
        StubGet("dids/9df99644-f1a5-4a3c-99a4-559d758eb96b", "dids/show.json");
        StubPatch("dids/9df99644-f1a5-4a3c-99a4-559d758eb96b", "dids/update.json");

        var did = (await Client.Dids().FindAsync("9df99644-f1a5-4a3c-99a4-559d758eb96b")).Data;
        did.Description = "patched from loaded resource";

        var response = await Client.Dids().UpdateAsync(did);

        response.Data.Description.Should().Be("something");
    }

    [Fact]
    public async Task TestUpdateDidSetVoiceInTrunkNullifiesTrunkGroup()
    {
        StubPatch("dids/9df99644-f1a5-4a3c-99a4-559d758eb96b", "dids/update.json");

        var did = Did.Build("9df99644-f1a5-4a3c-99a4-559d758eb96b");
        var trunk = VoiceInTrunk.Build("41b94706-325e-4704-a433-d65105758836");
        did.VoiceInTrunk = trunk;

        await Client.Dids().UpdateAsync(did);

        var requests = WireMock.LogEntries.Where(e =>
            e.RequestMessage.Method == "PATCH").ToList();
        requests.Should().HaveCount(1);
        var body = requests[0].RequestMessage.Body;
        var json = Newtonsoft.Json.Linq.JObject.Parse(body);
        var relationships = json["data"]!["relationships"]!;

        // voice_in_trunk_group should be nullified
        relationships["voice_in_trunk_group"]!["data"]!.Type.Should().Be(Newtonsoft.Json.Linq.JTokenType.Null);
    }

    [Fact]
    public async Task TestUpdateDidSetVoiceInTrunkGroupNullifiesTrunk()
    {
        StubPatch("dids/9df99644-f1a5-4a3c-99a4-559d758eb96b", "dids/update.json");

        var did = Did.Build("9df99644-f1a5-4a3c-99a4-559d758eb96b");
        var trunkGroup = VoiceInTrunkGroup.Build("some-trunk-group-id");
        did.VoiceInTrunkGroup = trunkGroup;

        await Client.Dids().UpdateAsync(did);

        var requests = WireMock.LogEntries.Where(e =>
            e.RequestMessage.Method == "PATCH").ToList();
        requests.Should().HaveCount(1);
        var body = requests[0].RequestMessage.Body;
        var json = Newtonsoft.Json.Linq.JObject.Parse(body);
        var relationships = json["data"]!["relationships"]!;

        // voice_in_trunk should be nullified
        relationships["voice_in_trunk"]!["data"]!.Type.Should().Be(Newtonsoft.Json.Linq.JTokenType.Null);
    }

    [Fact]
    public async Task TestIncludedResourcesHaveDirtyTracking()
    {
        StubGet("dids/9df99644-f1a5-4a3c-99a4-559d758eb96b", "dids/show_with_included_trunk.json");

        var queryParams = new QueryParams().Include("voice_in_trunk");
        var response = await Client.Dids().FindAsync("9df99644-f1a5-4a3c-99a4-559d758eb96b", queryParams);
        var did = response.Data;

        did.IsDirtyTrackingEnabled.Should().BeTrue();
        did.VoiceInTrunk.Should().NotBeNull();
        did.VoiceInTrunk!.IsDirtyTrackingEnabled.Should().BeTrue();
    }

    [Fact]
    public async Task TestFindDidWithAddressVerificationAndDidGroup()
    {
        StubGet("dids/21d0b02c-b556-4d3e-acbf-504b78295dbe", "dids/show_with_address_verification_and_did_group.json");

        var queryParams = new QueryParams().Include("address_verification", "did_group");
        var response = await Client.Dids().FindAsync("21d0b02c-b556-4d3e-acbf-504b78295dbe", queryParams);
        var did = response.Data;

        did.Number.Should().Be("61488943592");

        var addressVerification = did.AddressVerification;
        addressVerification.Should().NotBeNull();
        addressVerification!.Id.Should().Be("75dc8d39-5e17-4470-a6f3-df42642c975f");
        addressVerification.Status.Should().Be(AddressVerificationStatus.Approved);

        var didGroup = did.DidGroup;
        didGroup.Should().NotBeNull();
        didGroup!.Id.Should().Be("2b60bb9a-d382-4d35-84c6-61689f45f2f5");
        didGroup.Prefix.Should().Be("4");
        didGroup.AreaName.Should().Be("Mobile");
        didGroup.IsMetered.Should().BeFalse();
        didGroup.AllowAdditionalChannels.Should().BeFalse();
    }
}
