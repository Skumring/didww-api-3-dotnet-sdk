using Didww.Api3.Http;

namespace Didww.Api3.Examples;

public static class CountriesExample
{
    public static async Task RunAsync(DidwwClient client)
    {
        Console.WriteLine("--- List Countries ---");
        var response = await client.Countries().ListAsync();
        foreach (var country in response.Data.Take(5))
        {
            Console.WriteLine($"  {country.Name} ({country.Iso}) - Prefix: {country.Prefix}");
        }

        Console.WriteLine("\n--- Find Country with Regions ---");
        var queryParams = new QueryParams().Include("regions");
        var first = response.Data.First();
        var countryResponse = await client.Countries().FindAsync(first.Id!, queryParams);
        var c = countryResponse.Data;
        Console.WriteLine($"  {c.Name} has {c.Regions?.Count ?? 0} regions");
    }
}
