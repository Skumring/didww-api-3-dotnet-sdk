using Didww.Api3.Http;
using Didww.Api3.Resource;

namespace Didww.Api3.Examples;

public static class DidsExample
{
    public static async Task RunAsync(DidwwClient client)
    {
        // Get the last ordered DID (include 2026-04-16 emergency relationships)
        Console.WriteLine("--- List DIDs ---");
        var queryParams = new QueryParams()
            .Include("order", "identity", "emergency_calling_service", "emergency_verification")
            .Filter("blocked", "false")
            .Page(1, 5);
        var response = await client.Dids().ListAsync(queryParams);
        Console.WriteLine($"  Found {response.Data.Count} DIDs");
        foreach (var did in response.Data)
        {
            Console.WriteLine($"  DID: {did.Number} (Capacity: {did.CapacityLimit})");
            Console.WriteLine($"    Emergency enabled: {did.EmergencyEnabled}");
            if (did.EmergencyCallingService != null)
                Console.WriteLine($"    Emergency Calling Service: {did.EmergencyCallingService.Id}");
            if (did.EmergencyVerification != null)
                Console.WriteLine($"    Emergency Verification: {did.EmergencyVerification.Id}");
            if (did.Identity != null)
                Console.WriteLine($"    Identity: {did.Identity.Id}");
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
