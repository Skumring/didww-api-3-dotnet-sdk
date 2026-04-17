using Didww.Api3.Resource;

namespace Didww.Api3.Examples;

// Validates an emergency calling service order before placing it (2026-04-16).
//
// EmergencyRequirementValidation is a write-only endpoint: POST the
// intended (emergency_requirement, address, identity) triple and the server
// either returns 204 No Content (OK to order) or JSONAPI errors describing
// what the customer must fix (missing address fields, wrong identity type,
// unsupported area level, etc.).
public static class EmergencyRequirementValidationsExample
{
    public static async Task RunAsync(DidwwClient client)
    {
        // Pick any emergency requirement + address + identity from your account.
        var requirementsResponse = await client.EmergencyRequirements().ListAsync();
        var requirement = requirementsResponse.Data.FirstOrDefault();

        var addressesResponse = await client.Addresses().ListAsync();
        var address = addressesResponse.Data.FirstOrDefault();

        var identitiesResponse = await client.Identities().ListAsync();
        var identity = identitiesResponse.Data.FirstOrDefault();

        if (requirement == null)
        {
            Console.WriteLine("No emergency_requirements found on this account, skipping.");
            return;
        }
        if (address == null)
        {
            Console.WriteLine("No addresses found on this account, skipping.");
            return;
        }
        if (identity == null)
        {
            Console.WriteLine("No identities found on this account, skipping.");
            return;
        }

        Console.WriteLine("Validating order setup with:");
        Console.WriteLine($"  Emergency Requirement: {requirement.Id}");
        Console.WriteLine($"  Address:               {address.Id}");
        Console.WriteLine($"  Identity:              {identity.Id}");

        var validation = new EmergencyRequirementValidation
        {
            EmergencyRequirement = EmergencyRequirement.Build(requirement.Id!),
            Address = Address.Build(address.Id!),
            Identity = Identity.Build(identity.Id!)
        };

        try
        {
            var response = await client.EmergencyRequirementValidations().CreateAsync(validation);
            Console.WriteLine("\nValidation passed — this combination can be used to order emergency calling.");
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"\nValidation failed: {ex.Message}");
        }
    }
}
