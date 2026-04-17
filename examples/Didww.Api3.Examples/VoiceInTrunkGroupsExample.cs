using Didww.Api3.Http;
using Didww.Api3.Resource;

namespace Didww.Api3.Examples;

public static class VoiceInTrunkGroupsExample
{
    public static async Task RunAsync(DidwwClient client)
    {
        var suffix = Guid.NewGuid().ToString("N")[..8];

        // Create a new trunk group (2026-04-16 external_reference_id for customer tagging)
        Console.WriteLine("--- Create Trunk Group ---");
        var group = new VoiceInTrunkGroup
        {
            Name = "Dotnet Trunk Group " + suffix,
            CapacityLimit = 20,
            ExternalReferenceId = "dotnet-tg-" + suffix
        };

        group = (await client.VoiceInTrunkGroups().CreateAsync(group)).Data;
        Console.WriteLine($"  Created trunk group: {group.Id} - {group.Name}");
        Console.WriteLine($"    External reference: {group.ExternalReferenceId}");

        // List trunk groups
        Console.WriteLine("\n--- List Trunk Groups ---");
        var queryParams = new QueryParams().Include("voice_in_trunks");
        var listResponse = await client.VoiceInTrunkGroups().ListAsync(queryParams);
        Console.WriteLine($"  Found {listResponse.Data.Count} trunk groups");
        foreach (var g in listResponse.Data.Take(3))
        {
            var trunkCount = g.VoiceInTrunks?.Count ?? 0;
            Console.WriteLine($"  {g.Name} ({trunkCount} trunks)");
            if (g.ExternalReferenceId != null)
                Console.WriteLine($"    External reference: {g.ExternalReferenceId}");
        }

        // Update trunk group
        Console.WriteLine("\n--- Update Trunk Group ---");
        group.CapacityLimit = 30;
        var updated = (await client.VoiceInTrunkGroups().UpdateAsync(group)).Data;
        Console.WriteLine($"  Updated trunk group: {updated.Name} (capacity_limit: {updated.CapacityLimit})");

        // Delete trunk group
        Console.WriteLine("\n--- Delete Trunk Group ---");
        await client.VoiceInTrunkGroups().DeleteAsync(group.Id!);
        Console.WriteLine("  Trunk group deleted");
    }
}
