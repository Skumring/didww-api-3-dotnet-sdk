using Didww.Api3.Http;
using Didww.Api3.Resource.Enums;

namespace Didww.Api3.Examples;

// Lists DID ownership history (2026-04-16).
// Records are retained for the last 90 days only.
//
// Server-side filters supported:
//   did_number (eq), action (eq), method (eq),
//   created_at_gteq, created_at_lteq
public static class DidHistoryExample
{
    public static async Task RunAsync(DidwwClient client)
    {
        Console.WriteLine("=== Recent DID History ===");
        var response = await client.DidHistory().ListAsync();
        var events = response.Data;
        Console.WriteLine($"Found {events.Count} events in the last 90 days");

        foreach (var ev in events.Take(10))
        {
            Console.WriteLine($"  {ev.CreatedAt:o}  {ev.DidNumber,-16}  {ev.Action,-28}  via {ev.Method}");
        }

        // Filter by action
        Console.WriteLine("\n=== Only 'assigned' events ===");
        var assignedParams = new QueryParams().Filter("action", "assigned");
        var assigned = await client.DidHistory().ListAsync(assignedParams);
        Console.WriteLine($"Found {assigned.Data.Count} assignments");

        // Filter by a specific DID number
        if (events.Count > 0)
        {
            var number = events[0].DidNumber;
            Console.WriteLine($"\n=== History for DID {number} ===");
            var perNumberParams = new QueryParams().Filter("did_number", number!);
            var perNumber = await client.DidHistory().ListAsync(perNumberParams);
            foreach (var ev in perNumber.Data)
            {
                Console.WriteLine($"  {ev.CreatedAt:o}  {ev.Action}  via {ev.Method}");
            }
        }

        // Filter by date range (last 7 days)
        var sevenDaysAgo = DateTimeOffset.UtcNow.AddDays(-7).ToString("o");
        Console.WriteLine($"\n=== History since {sevenDaysAgo} ===");
        var recentParams = new QueryParams().Filter("created_at_gteq", sevenDaysAgo);
        var recent = await client.DidHistory().ListAsync(recentParams);
        Console.WriteLine($"Found {recent.Data.Count} events in the last 7 days");

        Console.WriteLine("\nAvailable actions: " +
            string.Join(", ", Enum.GetNames<DidHistoryAction>()));
        Console.WriteLine("Available methods: " +
            string.Join(", ", Enum.GetNames<DidHistoryMethod>()));
    }
}
