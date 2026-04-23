using Didww.Api3.Http;
using Didww.Api3.Resource;
using Didww.Api3.Resource.OrderItem;

namespace Didww.Api3.Examples;

public static class OrdersExample
{
    public static async Task RunAsync(DidwwClient client)
    {
        Console.WriteLine("--- Order DID by SKU ---");

        // Find a DID group with stock keeping units
        var queryParams = new QueryParams()
            .Include("stock_keeping_units")
            .Page(1, 1);
        var didGroups = await client.DidGroups().ListAsync(queryParams);
        if (didGroups.Data.Count == 0
            || didGroups.Data[0].StockKeepingUnits == null
            || didGroups.Data[0].StockKeepingUnits.Count == 0)
        {
            Console.WriteLine("  No DID group with stock_keeping_units found, skipping.");
            return;
        }

        var sku = didGroups.Data[0].StockKeepingUnits[0];
        Console.WriteLine($"  Using SKU: {sku.Id} (setup: {sku.SetupPrice}, monthly: {sku.MonthlyPrice})");

        var orderItem = new DidOrderItem
        {
            SkuId = sku.Id,
            Qty = 1
        };

        // 2026-04-16 external_reference_id — customer-supplied tag (max 100 chars)
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var order = new Order
        {
            AllowBackOrdering = true,
            Items = new List<OrderItemBase> { orderItem },
            ExternalReferenceId = "dotnet-order-" + suffix
        };

        var response = await client.Orders().CreateAsync(order);
        Console.WriteLine($"  Order created: {response.Data.Id} (Status: {response.Data.Status}, Amount: {response.Data.Amount})");
        Console.WriteLine($"  External reference: {response.Data.ExternalReferenceId}");

        Console.WriteLine("\n--- List Orders ---");
        var listResponse = await client.Orders().ListAsync(new QueryParams().Page(1, 5));
        foreach (var o in listResponse.Data)
        {
            Console.WriteLine($"  Order: {o.Id} (Status: {o.Status}, Amount: {o.Amount})");
            if (o.ExternalReferenceId != null)
                Console.WriteLine($"    External reference: {o.ExternalReferenceId}");
        }
    }
}
