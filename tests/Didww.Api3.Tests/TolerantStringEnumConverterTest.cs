using Didww.Api3.Resource.Enums;
using FluentAssertions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Didww.Api3.Tests;

public class TolerantStringEnumConverterTest : BaseTest
{
    [Fact]
    public async Task TestUnknownEnumValueDoesNotThrow()
    {
        // Stub a DidHistory response with an unknown action value
        var json = @"{
          ""data"": {
            ""id"": ""aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"",
            ""type"": ""did_history"",
            ""attributes"": {
              ""did_number"": ""442038680521"",
              ""action"": ""future_new_status"",
              ""method"": ""system"",
              ""created_at"": ""2026-04-15T09:00:00.000Z""
            }
          }
        }";

        WireMock.Given(
            Request.Create()
                .WithPath("/v3/did_history/aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
                .UsingGet()
        ).RespondWith(
            Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/vnd.api+json")
                .WithBody(json)
        );

        var response = await Client.DidHistory().FindAsync("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
        var entry = response.Data;

        // Should not throw and should fall back to the first enum member
        entry.Id.Should().Be("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
        entry.DidNumber.Should().Be("442038680521");
        entry.Action.Should().Be(DidHistoryAction.Assigned); // first enum value fallback
        entry.Method.Should().Be(DidHistoryMethod.System);
    }

    [Fact]
    public async Task TestKnownEnumValueStillWorks()
    {
        StubGet("did_history/01234567-89ab-cdef-0123-456789abcdef",
            "did_history/show.json");

        var response = await Client.DidHistory().FindAsync("01234567-89ab-cdef-0123-456789abcdef");
        var entry = response.Data;

        entry.Action.Should().Be(DidHistoryAction.Renewed);
    }
}
