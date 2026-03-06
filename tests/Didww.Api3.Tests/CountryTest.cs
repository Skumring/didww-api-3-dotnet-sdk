using Didww.Api3.Http;
using Didww.Api3.Resource;
using FluentAssertions;

namespace Didww.Api3.Tests;

public class CountryTest : BaseTest
{
    [Fact]
    public async Task TestListCountries()
    {
        StubGet("countries", "countries/index.json");

        var response = await Client.Countries().ListAsync();
        var countries = response.Data;

        countries.Should().NotBeEmpty();

        var first = countries[0];
        first.Id.Should().Be("6c7727b3-6e17-4b8b-a4b3-4c5142e31a63");
        first.Name.Should().Be("Afghanistan");
        first.Prefix.Should().Be("93");
        first.Iso.Should().Be("AF");
    }

    [Fact]
    public async Task TestFindCountry()
    {
        StubGet("countries/7eda11bb-0e66-4146-98e7-57a5281f56c8", "countries/show.json");

        var response = await Client.Countries().FindAsync("7eda11bb-0e66-4146-98e7-57a5281f56c8");
        var country = response.Data;

        country.Name.Should().Be("United Kingdom");
        country.Prefix.Should().Be("44");
        country.Iso.Should().Be("GB");
    }

    [Fact]
    public async Task TestFindCountryWithRegions()
    {
        StubGet("countries/661d8448-8897-4765-acda-00cc1740148d", "countries/show_with_regions.json");

        var queryParams = new QueryParams().Include("regions");
        var response = await Client.Countries().FindAsync("661d8448-8897-4765-acda-00cc1740148d", queryParams);
        var country = response.Data;

        country.Name.Should().Be("Lithuania");
        country.Prefix.Should().Be("370");
        country.Iso.Should().Be("LT");

        country.Regions.Should().HaveCount(10);
        country.Regions![0].Name.Should().Be("Alytaus Apskritis");
    }
}
