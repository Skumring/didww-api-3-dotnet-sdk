using Didww.Api3.Http;
using FluentAssertions;

namespace Didww.Api3.Tests;

public class QueryParamsTest
{
    [Fact]
    public void TestEmptyQueryParams()
    {
        var qp = new QueryParams();
        qp.ToQueryString().Should().Be("");
    }

    [Fact]
    public void TestFilter()
    {
        var qp = new QueryParams().Filter("number", "123456");
        qp.ToQueryString().Should().Contain("filter[number]=123456");
    }

    [Fact]
    public void TestMultipleFilters()
    {
        var qp = new QueryParams()
            .Filter("number", "123456")
            .Filter("blocked", "false");
        var qs = qp.ToQueryString();
        qs.Should().Contain("filter[number]=123456");
        qs.Should().Contain("filter[blocked]=false");
    }

    [Fact]
    public void TestInclude()
    {
        var qp = new QueryParams().Include("order", "did_group");
        qp.ToQueryString().Should().Contain("include=order%2Cdid_group");
    }

    [Fact]
    public void TestSort()
    {
        var qp = new QueryParams().Sort("name", "-created_at");
        qp.ToQueryString().Should().Contain("sort=name%2C-created_at");
    }

    [Fact]
    public void TestPage()
    {
        var qp = new QueryParams().Page(2, 25);
        var qs = qp.ToQueryString();
        qs.Should().Contain("page[number]=2");
        qs.Should().Contain("page[size]=25");
    }

    [Fact]
    public void TestCombined()
    {
        var qp = new QueryParams()
            .Filter("number", "123")
            .Include("order")
            .Sort("name")
            .Page(1, 10);
        var qs = qp.ToQueryString();
        qs.Should().StartWith("?");
        qs.Should().Contain("filter[number]=123");
        qs.Should().Contain("include=order");
        qs.Should().Contain("sort=name");
        qs.Should().Contain("page[number]=1");
        qs.Should().Contain("page[size]=10");
    }
}
