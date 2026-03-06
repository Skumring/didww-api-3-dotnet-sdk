using Didww.Api3;
using Didww.Api3.Callback;
using Didww.Api3.Http;
using Didww.Api3.Resource;
using Didww.Api3.Resource.Configuration;
using Didww.Api3.Resource.Enums;
using Didww.Api3.Resource.OrderItem;

// =====================================================
// DIDWW API v3 .NET SDK Examples
// =====================================================
// NOTE: Replace "your-api-key" with your actual API key
// and adjust environment as needed.
// =====================================================

Console.WriteLine("DIDWW API v3 .NET SDK Examples");
Console.WriteLine("==============================\n");

// --- 1. Client Setup ---
var client = DidwwClient.NewBuilder()
    .SetCredentials(new DidwwCredentials("your-api-key", DidwwEnvironment.Sandbox))
    .SetTimeout(TimeSpan.FromSeconds(30))
    .Build();

Console.WriteLine($"Client configured for: {client.BaseUrl}\n");

// --- 2. List Countries ---
Console.WriteLine("--- List Countries ---");
try
{
    var countriesResponse = await client.Countries().ListAsync();
    foreach (var country in countriesResponse.Data.Take(5))
    {
        Console.WriteLine($"  {country.Name} ({country.Iso}) - Prefix: {country.Prefix}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"  Error: {ex.Message}");
}

// --- 3. Find Country with Regions ---
Console.WriteLine("\n--- Find Country with Regions ---");
try
{
    var queryParams = new QueryParams().Include("regions");
    var countryResponse = await client.Countries().FindAsync("661d8448-8897-4765-acda-00cc1740148d", queryParams);
    var country = countryResponse.Data;
    Console.WriteLine($"  {country.Name} has {country.Regions?.Count ?? 0} regions");
}
catch (Exception ex)
{
    Console.WriteLine($"  Error: {ex.Message}");
}

// --- 4. Check Balance ---
Console.WriteLine("\n--- Check Balance ---");
try
{
    var balanceResponse = await client.Balance().FindAsync();
    var balance = balanceResponse.Data;
    Console.WriteLine($"  Balance: {balance.BalanceAmount}, Credit: {balance.Credit}, Total: {balance.TotalBalance}");
}
catch (Exception ex)
{
    Console.WriteLine($"  Error: {ex.Message}");
}

// --- 5. List DIDs with filtering ---
Console.WriteLine("\n--- List DIDs ---");
try
{
    var didParams = new QueryParams()
        .Include("order")
        .Filter("blocked", "false")
        .Page(1, 5);
    var didsResponse = await client.Dids().ListAsync(didParams);
    foreach (var did in didsResponse.Data)
    {
        Console.WriteLine($"  DID: {did.Number} (Capacity: {did.CapacityLimit})");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"  Error: {ex.Message}");
}

// --- 6. Create Order ---
Console.WriteLine("\n--- Create Order ---");
try
{
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

    var orderResponse = await client.Orders().CreateAsync(order);
    Console.WriteLine($"  Order created: {orderResponse.Data.Id} (Status: {orderResponse.Data.Status})");
}
catch (Exception ex)
{
    Console.WriteLine($"  Error: {ex.Message}");
}

// --- 7. Create Voice In Trunk ---
Console.WriteLine("\n--- Create Voice In Trunk ---");
try
{
    var sipConfig = new SipConfiguration
    {
        Username = "myuser",
        Host = "192.168.1.1",
        Port = 5060,
        CodecIds = new List<Codec> { Codec.PCMU, Codec.PCMA }
    };

    var trunk = new VoiceInTrunk
    {
        Name = "My SIP Trunk",
        Configuration = sipConfig
    };

    var trunkResponse = await client.VoiceInTrunks().CreateAsync(trunk);
    Console.WriteLine($"  Trunk created: {trunkResponse.Data.Id} ({trunkResponse.Data.Name})");
}
catch (Exception ex)
{
    Console.WriteLine($"  Error: {ex.Message}");
}

// --- 8. Update DID ---
Console.WriteLine("\n--- Update DID ---");
try
{
    var did = Did.Build("your-did-id");
    did.Description = "Updated via SDK";
    did.CapacityLimit = 5;

    var updateResponse = await client.Dids().UpdateAsync(did);
    Console.WriteLine($"  DID updated: {updateResponse.Data.Id}");
}
catch (Exception ex)
{
    Console.WriteLine($"  Error: {ex.Message}");
}

// --- 9. Encryption Example ---
Console.WriteLine("\n--- Encryption ---");
try
{
    var encrypt = new Encrypt(client);
    Console.WriteLine($"  Fingerprint: {encrypt.Fingerprint}");

    var fileData = System.Text.Encoding.UTF8.GetBytes("sensitive document content");
    var encryptedData = encrypt.EncryptData(fileData);
    Console.WriteLine($"  Encrypted {fileData.Length} bytes -> {encryptedData.Length} bytes");

    var fileIds = await client.UploadEncryptedFileAsync(encryptedData, "document.pdf", encrypt.Fingerprint, "Test document");
    Console.WriteLine($"  Uploaded encrypted file IDs: {string.Join(", ", fileIds)}");
}
catch (Exception ex)
{
    Console.WriteLine($"  Error: {ex.Message}");
}

// --- 10. Webhook Callback Validation ---
Console.WriteLine("\n--- Webhook Callback Validation ---");
var validator = new RequestValidator("your-api-key");
var callbackUrl = "https://example.com/webhook";
var callbackPayload = new Dictionary<string, string>
{
    { "status", "completed" },
    { "id", "order-id" },
    { "type", "orders" }
};
var isValid = validator.Validate(callbackUrl, callbackPayload, "some-signature");
Console.WriteLine($"  Signature valid: {isValid}");

Console.WriteLine("\nDone!");
