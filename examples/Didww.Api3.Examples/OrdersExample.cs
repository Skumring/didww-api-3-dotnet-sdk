using Didww.Api3.Resource;
using Didww.Api3.Resource.OrderItem;

namespace Didww.Api3.Examples;

public static class OrdersExample
{
    public static async Task RunAsync(DidwwClient client)
    {
        Console.WriteLine("--- Create Order ---");
        var orderItem = new DidOrderItem
        {
            SkuId = "your-sku-id",
            Qty = 1
        };

        var order = new Order
        {
            AllowBackOrdering = true,
            Items = new List<OrderItemBase> { orderItem }
        };

        var response = await client.Orders().CreateAsync(order);
        Console.WriteLine($"  Order created: {response.Data.Id} (Status: {response.Data.Status})");

        Console.WriteLine("\n--- List Orders ---");
        var listResponse = await client.Orders().ListAsync();
        foreach (var o in listResponse.Data)
        {
            Console.WriteLine($"  Order: {o.Id} (Status: {o.Status}, Amount: {o.Amount})");
        }
    }
}
