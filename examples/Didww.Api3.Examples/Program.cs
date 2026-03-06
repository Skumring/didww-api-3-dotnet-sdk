using Didww.Api3.Examples;

// DIDWW API v3 .NET SDK Examples
// Set DIDWW_API_KEY environment variable before running.
// Usage: dotnet run [example-name]
// Available examples: balance, countries, dids, trunks, orders, exports, encryption, webhook

var exampleName = args.Length > 0 ? args[0].ToLower() : "all";
var client = exampleName != "webhook" ? ExampleClientFactory.Create() : null;

try
{
    switch (exampleName)
    {
        case "balance":
            await BalanceExample.RunAsync(client!);
            break;
        case "countries":
            await CountriesExample.RunAsync(client!);
            break;
        case "dids":
            await DidsExample.RunAsync(client!);
            break;
        case "trunks":
            await TrunksExample.RunAsync(client!);
            break;
        case "orders":
            await OrdersExample.RunAsync(client!);
            break;
        case "exports":
            await ExportsExample.RunAsync(client!);
            break;
        case "encryption":
            await EncryptionExample.RunAsync(client!);
            break;
        case "webhook":
            WebhookExample.Run();
            break;
        case "all":
            await BalanceExample.RunAsync(client!);
            Console.WriteLine();
            await CountriesExample.RunAsync(client!);
            Console.WriteLine();
            await DidsExample.RunAsync(client!);
            Console.WriteLine();
            await TrunksExample.RunAsync(client!);
            Console.WriteLine();
            await OrdersExample.RunAsync(client!);
            Console.WriteLine();
            await ExportsExample.RunAsync(client!);
            Console.WriteLine();
            await EncryptionExample.RunAsync(client!);
            Console.WriteLine();
            WebhookExample.Run();
            break;
        default:
            Console.WriteLine($"Unknown example: {exampleName}");
            Console.WriteLine("Available: balance, countries, dids, trunks, orders, exports, encryption, webhook, all");
            break;
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
