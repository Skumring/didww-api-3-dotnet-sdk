using Didww.Api3.Http;
using Didww.Api3.Resource;
using Didww.Api3.Resource.Enums;

namespace Didww.Api3.Examples;

// End-to-end Emergency Calling Service purchase flow (2026-04-16).
//
// Steps:
//   1. Find a DID with the "emergency" feature (via its DID group).
//   2. Get emergency requirements for that DID group type + country.
//   3. Find an identity and address on the account.
//   4. Validate the (requirement, address, identity) triple.
//   5. Create an emergency verification.
//   6. Check verification status.
//   7. List emergency calling services to see the result.
public static class EmergencyScenarioExample
{
    public static async Task RunAsync(DidwwClient client)
    {
        Console.WriteLine("=== Emergency Scenario: End-to-End Flow ===\n");

        // Step 1: Find a DID whose DID group supports the "emergency" feature.
        Console.WriteLine("--- Step 1: Find a DID with emergency feature ---");
        var didParams = new QueryParams()
            .Include("did_group.country", "did_group.did_group_type", "emergency_calling_service")
            .Filter("did_group.features.emergency", "true")
            .Filter("blocked", "false")
            .Page(1, 1);
        var didResponse = await client.Dids().ListAsync(didParams);
        var did = didResponse.Data.FirstOrDefault();

        if (did == null)
        {
            Console.WriteLine("No DID with emergency feature found on this account, skipping.");
            return;
        }

        Console.WriteLine($"  DID: {did.Number} (ID: {did.Id})");
        Console.WriteLine($"  Emergency enabled: {did.EmergencyEnabled}");
        var didGroup = did.DidGroup;
        if (didGroup != null)
        {
            Console.WriteLine($"  DID Group: {didGroup.Id} ({didGroup.AreaName})");
            Console.WriteLine($"  Country: {didGroup.Country?.Name}");
            Console.WriteLine($"  DID Group Type: {didGroup.DidGroupType?.Name}");
        }

        // Step 2: Get emergency requirements for this country + DID group type.
        Console.WriteLine("\n--- Step 2: Get emergency requirements ---");
        var reqParams = new QueryParams()
            .Include("country", "did_group_type");
        if (didGroup?.Country != null)
            reqParams.Filter("country.id", didGroup.Country.Id!);
        if (didGroup?.DidGroupType != null)
            reqParams.Filter("did_group_type.id", didGroup.DidGroupType.Id!);

        var reqResponse = await client.EmergencyRequirements().ListAsync(reqParams);
        var requirement = reqResponse.Data.FirstOrDefault();

        if (requirement == null)
        {
            Console.WriteLine("No emergency requirements found for this DID group, skipping.");
            return;
        }

        Console.WriteLine($"  Requirement: {requirement.Id}");
        Console.WriteLine($"  Identity type: {requirement.IdentityType}");
        Console.WriteLine($"  Address area level: {requirement.AddressAreaLevel}");
        if (requirement.AddressMandatoryFields != null)
            Console.WriteLine($"  Mandatory fields: {string.Join(", ", requirement.AddressMandatoryFields)}");
        Console.WriteLine($"  Estimated setup time (days): {requirement.EstimateSetupTime}");

        // Step 3: Find an identity and address on the account.
        Console.WriteLine("\n--- Step 3: Find identity and address ---");
        var identitiesResponse = await client.Identities().ListAsync();
        var identity = identitiesResponse.Data.FirstOrDefault();

        if (identity == null)
        {
            Console.WriteLine("No identities found on this account, skipping.");
            return;
        }
        Console.WriteLine($"  Identity: {identity.Id}");

        var addressesResponse = await client.Addresses().ListAsync();
        var address = addressesResponse.Data.FirstOrDefault();

        if (address == null)
        {
            Console.WriteLine("No addresses found on this account, skipping.");
            return;
        }
        Console.WriteLine($"  Address: {address.Id}");

        // Step 4: Validate the (requirement, address, identity) triple.
        Console.WriteLine("\n--- Step 4: Validate emergency requirement ---");
        var validation = new EmergencyRequirementValidation
        {
            EmergencyRequirement = EmergencyRequirement.Build(requirement.Id!),
            Address = Address.Build(address.Id!),
            Identity = Identity.Build(identity.Id!)
        };

        try
        {
            await client.EmergencyRequirementValidations().CreateAsync(validation);
            Console.WriteLine("  Validation passed — this combination can be used to order emergency calling.");
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"  Validation failed: {ex.Message}");
            Console.WriteLine("  (Fix the issues above and retry, or try different identity/address.)");
            return;
        }

        // Step 5: Create an emergency verification.
        Console.WriteLine("\n--- Step 5: Create emergency verification ---");

        // First check if there is already an active emergency calling service for this DID.
        if (did.EmergencyCallingService != null)
        {
            Console.WriteLine($"  DID already has Emergency Calling Service: {did.EmergencyCallingService.Id}");
            Console.WriteLine("  Skipping verification creation.");
        }
        else
        {
            Console.WriteLine("  (Verification creation requires an existing EmergencyCallingService.)");
            Console.WriteLine("  To create one, place an order with an EmergencyOrderItem first.");
            Console.WriteLine("  See OrdersEmergencyExample for details.");
        }

        // Step 6: Check verification status.
        Console.WriteLine("\n--- Step 6: Check emergency verification status ---");
        var evParams = new QueryParams()
            .Include("address", "emergency_calling_service", "dids")
            .Page(1, 5);
        var evResponse = await client.EmergencyVerifications().ListAsync(evParams);

        if (evResponse.Data.Count == 0)
        {
            Console.WriteLine("  No emergency verifications found on this account.");
        }
        else
        {
            foreach (var ev in evResponse.Data.Take(3))
            {
                Console.WriteLine($"  Verification: {ev.Id}");
                Console.WriteLine($"    Status: {ev.Status}");
                Console.WriteLine($"    Reference: {ev.Reference}");
                if (ev.ExternalReferenceId != null)
                    Console.WriteLine($"    External Reference: {ev.ExternalReferenceId}");
                if (ev.IsRejected && ev.RejectReasons != null)
                    Console.WriteLine($"    Reject reasons: {string.Join(", ", ev.RejectReasons)}");
            }
        }

        // Step 7: List emergency calling services.
        Console.WriteLine("\n--- Step 7: List emergency calling services ---");
        var ecsParams = new QueryParams()
            .Include("country", "did_group_type", "dids", "address")
            .Page(1, 5);
        var ecsResponse = await client.EmergencyCallingServices().ListAsync(ecsParams);

        if (ecsResponse.Data.Count == 0)
        {
            Console.WriteLine("  No emergency calling services found on this account.");
        }
        else
        {
            Console.WriteLine($"  Found {ecsResponse.Data.Count} emergency calling services");
            foreach (var svc in ecsResponse.Data.Take(3))
            {
                Console.WriteLine($"\n  Service: {svc.Id}");
                Console.WriteLine($"    Name: {svc.Name}");
                Console.WriteLine($"    Status: {svc.Status}");
                Console.WriteLine($"    Country: {svc.Country?.Name}");
                Console.WriteLine($"    DID Group Type: {svc.DidGroupType?.Name}");
                if (svc.Address != null)
                    Console.WriteLine($"    Address: {svc.Address.Id}");
                if (svc.Dids != null && svc.Dids.Count > 0)
                    Console.WriteLine($"    Attached DIDs: {string.Join(", ", svc.Dids.Select(d => d.Number))}");
            }
        }

        Console.WriteLine("\n=== Emergency Scenario Complete ===");
    }
}
