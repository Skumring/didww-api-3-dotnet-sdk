using Didww.Api3.Resource;
using Didww.Api3.Resource.Enums;

namespace Didww.Api3.Examples;

// Creates and lists CDR exports (cdr_in / cdr_out).
//
// 2026-04-16 additions:
//   * external_reference_id — customer-supplied reference (max 100 chars)
//
// Filter semantics on CDR exports:
//   * filters.from — lower bound, INCLUSIVE  (server: time_start >= from)
//   * filters.to   — upper bound, EXCLUSIVE  (server: time_start <  to)
// To cover a whole day, pass from: "2026-04-15 00:00:00", to: "2026-04-16 00:00:00".
public static class ExportsExample
{
    public static async Task RunAsync(DidwwClient client)
    {
        Console.WriteLine("=== Existing Exports ===");
        var listResponse = await client.Exports().ListAsync();
        Console.WriteLine($"Found {listResponse.Data.Count} exports");

        foreach (var e in listResponse.Data.Take(5))
        {
            Console.WriteLine($"Export: {e.Id}");
            Console.WriteLine($"  Type: {e.ExportType}");
            Console.WriteLine($"  Status: {e.Status}");
            Console.WriteLine($"  Created: {e.CreatedAt}");
            if (e.Url != null)
                Console.WriteLine($"  URL: {e.Url}");
            if (e.CallbackUrl != null)
                Console.WriteLine($"  Callback URL: {e.CallbackUrl}");
            if (e.ExternalReferenceId != null)
                Console.WriteLine($"  External Reference: {e.ExternalReferenceId}");
            Console.WriteLine();
        }

        // Create a CDR-In export for yesterday (from is inclusive, to is exclusive)
        Console.WriteLine("\n=== Creating CDR-In Export (yesterday, 2026-04-16 external_reference_id) ===");
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var yesterday = today.AddDays(-1);
        var suffix = Guid.NewGuid().ToString("N")[..8];

        var export = new Export
        {
            ExportType = ExportType.CdrIn,
            Filters = new Dictionary<string, object>
            {
                { "from", $"{yesterday:yyyy-MM-dd} 00:00:00" }, // inclusive  (time_start >= this)
                { "to",   $"{today:yyyy-MM-dd} 00:00:00" }      // exclusive  (time_start <  this)
            },
            ExternalReferenceId = "dotnet-cdr-in-" + suffix
        };

        var response = await client.Exports().CreateAsync(export);
        var created = response.Data;
        Console.WriteLine($"Created CDR-In export: {created.Id}");
        Console.WriteLine($"  External Reference: {created.ExternalReferenceId}");
        Console.WriteLine($"  Status: {created.Status}");

        // Find and inspect a specific export
        if (listResponse.Data.Count > 0)
        {
            Console.WriteLine("\n=== Specific Export Details ===");
            var specificExport = await client.Exports().FindAsync(listResponse.Data[0].Id!);
            Console.WriteLine($"Export: {specificExport.Data.Id}");
            Console.WriteLine($"  Type: {specificExport.Data.ExportType}");
            Console.WriteLine($"  Status: {specificExport.Data.Status}");
            Console.WriteLine($"  Created: {specificExport.Data.CreatedAt}");
            if (specificExport.Data.ExternalReferenceId != null)
                Console.WriteLine($"  External Reference: {specificExport.Data.ExternalReferenceId}");
            if (specificExport.Data.Filters != null)
            {
                Console.WriteLine("  Filters:");
                if (specificExport.Data.Filters.TryGetValue("from", out var from))
                    Console.WriteLine($"    From (inclusive): {from}");
                if (specificExport.Data.Filters.TryGetValue("to", out var to))
                    Console.WriteLine($"    To (exclusive):   {to}");
            }
        }

        if (created.Url != null)
        {
            Console.WriteLine("\n--- Download Export ---");
            var filePath = Path.Combine(Path.GetTempPath(), $"export_{created.Id}.csv");
            await client.DownloadExportAsync(created, filePath);
            Console.WriteLine($"  Downloaded to: {filePath}");
        }
    }
}
