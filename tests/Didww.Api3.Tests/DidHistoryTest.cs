using Didww.Api3.Resource.Enums;
using FluentAssertions;

namespace Didww.Api3.Tests;

public class DidHistoryTest : BaseTest
{
    [Fact]
    public async Task TestListDidHistory()
    {
        StubGet("did_history", "did_history/index.json");

        var response = await Client.DidHistory().ListAsync();
        var history = response.Data;

        history.Should().HaveCount(2);

        var first = history[0];
        first.Id.Should().Be("11111111-2222-3333-4444-555555555555");
        first.DidNumber.Should().Be("442038680521");
        first.Action.Should().Be(DidHistoryAction.Assigned);
        first.Method.Should().Be(DidHistoryMethod.Api3);
        first.CreatedAt.Should().NotBeNull();

        var second = history[1];
        second.Action.Should().Be(DidHistoryAction.Renewed);
        second.Method.Should().Be(DidHistoryMethod.System);
    }

    [Fact]
    public async Task TestFindDidHistory()
    {
        StubGet("did_history/01234567-89ab-cdef-0123-456789abcdef",
            "did_history/show.json");

        var response = await Client.DidHistory().FindAsync("01234567-89ab-cdef-0123-456789abcdef");
        var entry = response.Data;

        entry.Id.Should().Be("01234567-89ab-cdef-0123-456789abcdef");
        entry.DidNumber.Should().Be("442038680521");
        entry.Action.Should().Be(DidHistoryAction.Renewed);
        entry.Method.Should().Be(DidHistoryMethod.System);
    }

    [Fact]
    public async Task TestFindDidHistoryBillingCyclesCountChanged()
    {
        StubGet("did_history/aabbccdd-1122-3344-5566-778899aabbcc",
            "did_history/show_billing_cycles.json");

        var response = await Client.DidHistory().FindAsync("aabbccdd-1122-3344-5566-778899aabbcc");
        var entry = response.Data;

        entry.Id.Should().Be("aabbccdd-1122-3344-5566-778899aabbcc");
        entry.DidNumber.Should().Be("442038680521");
        entry.Action.Should().Be(DidHistoryAction.BillingCyclesCountChanged);
        entry.Method.Should().Be(DidHistoryMethod.Api3);
        entry.Meta.Should().NotBeNull();
        entry.Meta!["from"]!.ToString().Should().Be("2");
        entry.Meta!["to"]!.ToString().Should().Be("1");
    }
}
