using Didww.Api3.Resource;
using Didww.Api3.Resource.Enums;

namespace Didww.Api3.Examples;

public static class ExportsExample
{
    public static async Task RunAsync(DidwwClient client)
    {
        Console.WriteLine("--- Create CDR Export ---");
        var export = new Export
        {
            ExportType = ExportType.CdrIn,
            Filters = new Dictionary<string, object>
            {
                { "year", 2024 },
                { "month", 1 }
            }
        };

        var response = await client.Exports().CreateAsync(export);
        Console.WriteLine($"  Export created: {response.Data.Id} (Status: {response.Data.Status})");

        Console.WriteLine("\n--- List Exports ---");
        var listResponse = await client.Exports().ListAsync();
        foreach (var e in listResponse.Data)
        {
            Console.WriteLine($"  Export: {e.Id} (Type: {e.ExportType}, Status: {e.Status})");
        }

        if (response.Data.Url != null)
        {
            Console.WriteLine("\n--- Download Export ---");
            var filePath = Path.Combine(Path.GetTempPath(), $"export_{response.Data.Id}.csv");
            await client.DownloadExportAsync(response.Data, filePath);
            Console.WriteLine($"  Downloaded to: {filePath}");
        }
    }
}
