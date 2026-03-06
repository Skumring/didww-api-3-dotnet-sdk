# DIDWW API v3 .NET SDK

[![Tests](https://github.com/didww/didww-api-3-dotnet-sdk/actions/workflows/tests.yml/badge.svg)](https://github.com/didww/didww-api-3-dotnet-sdk/actions/workflows/tests.yml)
![Coverage](https://didww.github.io/didww-api-3-dotnet-sdk/coverage/coverage.svg)

.NET SDK for the [DIDWW API v3](https://doc.didww.com).

## Requirements

- .NET 9.0 or later

## Installation

```bash
dotnet add package Didww.Api3
```

## Configuration

```csharp
using Didww.Api3;

var client = DidwwClient.NewBuilder()
    .SetCredentials(new DidwwCredentials("your-api-key", DidwwEnvironment.Production))
    .SetTimeout(TimeSpan.FromSeconds(30))
    .Build();
```

### Sandbox Environment

```csharp
var client = DidwwClient.NewBuilder()
    .SetCredentials(new DidwwCredentials("your-api-key", DidwwEnvironment.Sandbox))
    .Build();
```

## Usage

### List Resources

```csharp
using Didww.Api3.Http;
using Didww.Api3.Resource;

// List countries
var countries = await client.Countries().ListAsync();
foreach (var country in countries.Data)
{
    Console.WriteLine($"{country.Name} ({country.Iso})");
}

// List with filtering, sorting, and pagination
var queryParams = new QueryParams()
    .Filter("number", "123456")
    .Include("order", "did_group")
    .Sort("-created_at")
    .Page(1, 25);

var dids = await client.Dids().ListAsync(queryParams);
```

### Find Resource

```csharp
var country = await client.Countries().FindAsync("country-id");
Console.WriteLine(country.Data.Name);

// With included relationships
var qp = new QueryParams().Include("regions");
var countryWithRegions = await client.Countries().FindAsync("country-id", qp);
```

### Create Resource

```csharp
using Didww.Api3.Resource.OrderItem;

var item = new DidOrderItem
{
    SkuId = "sku-id",
    Qty = 1
};

var order = new Order
{
    AllowBackOrdering = true,
    Items = new List<OrderItemBase> { item }
};

var response = await client.Orders().CreateAsync(order);
order = response.Data;
Console.WriteLine($"Order {order.Id} status: {order.Status}");
```

### Update Resource

The SDK tracks dirty fields — only modified attributes are sent in PATCH requests.

```csharp
// Update from a loaded resource (only changed fields are sent)
var did = (await client.Dids().FindAsync("did-id")).Data;
did.Description = "Updated description";
did.CapacityLimit = 5;
did = (await client.Dids().UpdateAsync(did)).Data;

// Update using Build (create reference by ID)
var didRef = Did.Build("did-id");
didRef.Description = "New description";
await client.Dids().UpdateAsync(didRef);
```

### Delete Resource

```csharp
await client.Dids().DeleteAsync("did-id");
```

### Voice Trunks

```csharp
using Didww.Api3.Resource.Configuration;
using Didww.Api3.Resource.Enums;

// Create SIP trunk
var sipConfig = new SipConfiguration
{
    Username = "myuser",
    Host = "192.168.1.1",
    Port = 5060,
    CodecIds = new List<Codec> { Codec.PCMU, Codec.PCMA, Codec.G729 }
};

var trunk = new VoiceInTrunk
{
    Name = "My SIP Trunk",
    Configuration = sipConfig
};

var trunkResponse = await client.VoiceInTrunks().CreateAsync(trunk);

// Create PSTN trunk
var pstnConfig = new PstnConfiguration { Dst = "558540420024" };
var pstnTrunk = new VoiceInTrunk
{
    Name = "My PSTN Trunk",
    Configuration = pstnConfig
};
await client.VoiceInTrunks().CreateAsync(pstnTrunk);
```

### Check Balance

```csharp
var balance = (await client.Balance().FindAsync()).Data;
Console.WriteLine($"Balance: {balance.BalanceAmount}, Credit: {balance.Credit}");
```

### File Encryption

```csharp
var encrypt = new Encrypt(client);

byte[] fileData = File.ReadAllBytes("document.pdf");
byte[] encryptedData = encrypt.EncryptData(fileData);

var fileIds = await client.UploadEncryptedFileAsync(
    encryptedData,
    "document.pdf",
    encrypt.Fingerprint,
    "My document"
);
```

### Webhook Callback Validation

```csharp
using Didww.Api3.Callback;

var validator = new RequestValidator("your-api-key");
var isValid = validator.Validate(
    "https://example.com/webhook",
    new Dictionary<string, string>
    {
        { "status", "completed" },
        { "id", "order-id" },
        { "type", "orders" }
    },
    request.Headers["X-DIDWW-Signature"]
);
```

## Available Resources

| Repository | Type | Operations |
|---|---|---|
| `Countries()` | Read-only | List, Find |
| `Regions()` | Read-only | List, Find |
| `Cities()` | Read-only | List, Find |
| `Areas()` | Read-only | List, Find |
| `Pops()` | Read-only | List, Find |
| `DidGroupTypes()` | Read-only | List, Find |
| `DidGroups()` | Read-only | List, Find |
| `AvailableDids()` | Read-only | List, Find |
| `NanpaPrefixes()` | Read-only | List, Find |
| `ProofTypes()` | Read-only | List, Find |
| `PublicKeys()` | Read-only | List, Find |
| `Requirements()` | Read-only | List, Find |
| `SupportingDocumentTemplates()` | Read-only | List, Find |
| `Balance()` | Singleton | Find |
| `Dids()` | CRUD | List, Find, Create, Update, Delete |
| `VoiceInTrunks()` | CRUD | List, Find, Create, Update, Delete |
| `VoiceInTrunkGroups()` | CRUD | List, Find, Create, Update, Delete |
| `VoiceOutTrunks()` | CRUD | List, Find, Create, Update, Delete |
| `Orders()` | CRUD | List, Find, Create, Update, Delete |
| `DidReservations()` | CRUD | List, Find, Create, Update, Delete |
| `CapacityPools()` | CRUD | List, Find, Create, Update, Delete |
| `SharedCapacityGroups()` | CRUD | List, Find, Create, Update, Delete |
| `Exports()` | CRUD | List, Find, Create, Update, Delete |
| `Addresses()` | CRUD | List, Find, Create, Update, Delete |
| `AddressVerifications()` | CRUD | List, Find, Create, Update, Delete |
| `Identities()` | CRUD | List, Find, Create, Update, Delete |
| `EncryptedFiles()` | CRUD | List, Find, Create, Update, Delete |
| `PermanentSupportingDocuments()` | CRUD | List, Find, Create, Update, Delete |
| `Proofs()` | CRUD | List, Find, Create, Update, Delete |
| `RequirementValidations()` | CRUD | List, Find, Create, Update, Delete |

## Development

```bash
# Run tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Build
dotnet build
```

## License

MIT License. See [LICENSE](LICENSE) for details.
