using Didww.Api3.Http;
using Didww.Api3.Resource;

namespace Didww.Api3.Examples;

public static class DidsExample
{
    public static async Task RunAsync(DidwwClient client)
    {
        Console.WriteLine("--- List DIDs ---");
        var queryParams = new QueryParams()
            .Include("order")
            .Filter("blocked", "false")
            .Page(1, 5);
        var response = await client.Dids().ListAsync(queryParams);
        foreach (var did in response.Data)
        {
            Console.WriteLine($"  DID: {did.Number} (Capacity: {did.CapacityLimit})");
        }

        if (response.Data.Count > 0)
        {
            Console.WriteLine("\n--- Update DID ---");
            var did = Did.Build(response.Data[0].Id!);
            did.Description = "Updated via SDK";
            did.CapacityLimit = 5;
            var updateResponse = await client.Dids().UpdateAsync(did);
            Console.WriteLine($"  DID updated: {updateResponse.Data.Id}");
        }
    }
}
