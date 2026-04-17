using Didww.Api3.Http;
using Didww.Api3.Resource;
using Didww.Api3.Resource.Enums;

namespace Didww.Api3.Examples;

// Creates and lists Emergency Verifications (2026-04-16).
//
// An EmergencyVerification submits an address and a list of DIDs against an
// EmergencyCallingService for compliance review. The server returns a status
// of "pending" and later moves to "approved" / "rejected".
//
// Supported operations: index, show, create.
public static class EmergencyVerificationsExample
{
    public static async Task RunAsync(DidwwClient client)
    {
        Console.WriteLine("=== Emergency Verifications ===");
        var queryParams = new QueryParams().Include("address", "emergency_calling_service", "dids");
        var response = await client.EmergencyVerifications().ListAsync(queryParams);
        var verifications = response.Data;
        Console.WriteLine($"Found {verifications.Count} emergency verifications");

        foreach (var ev in verifications.Take(5))
        {
            Console.WriteLine($"\nVerification: {ev.Id}");
            Console.WriteLine($"  Reference: {ev.Reference}");
            Console.WriteLine($"  Status: {ev.Status}");
            if (ev.ExternalReferenceId != null)
                Console.WriteLine($"  External Reference: {ev.ExternalReferenceId}");
            Console.WriteLine($"  Address: {ev.Address?.Id}");
            Console.WriteLine($"  Emergency Calling Service: {ev.EmergencyCallingService?.Id}");
            if (ev.Dids != null && ev.Dids.Count > 0)
                Console.WriteLine($"  DIDs: {string.Join(", ", ev.Dids.Select(d => d.Number))}");
            if (ev.IsRejected)
            {
                if (ev.RejectReasons != null)
                    Console.WriteLine($"  Reject reasons: {string.Join(", ", ev.RejectReasons)}");
                if (ev.RejectComment != null)
                    Console.WriteLine($"  Reject comment: {ev.RejectComment}");
            }
        }

        // Filter by status
        Console.WriteLine("\n=== Only approved verifications ===");
        var approvedParams = new QueryParams().Filter("status", "approved");
        var approved = await client.EmergencyVerifications().ListAsync(approvedParams);
        Console.WriteLine($"Found {approved.Data.Count} approved verifications");

        // Create a new verification — requires an existing EmergencyCallingService,
        // Address, and Did. Uncomment and replace the IDs below to try:
        //
        // var suffix = Guid.NewGuid().ToString("N")[..8];
        // var verification = new EmergencyVerification
        // {
        //     Address = Address.Build("<address-uuid>"),
        //     EmergencyCallingService = EmergencyCallingService.Build("<ecs-uuid>"),
        //     Dids = new List<Did> { Did.Build("<did-uuid>") },
        //     ExternalReferenceId = "dotnet-ev-" + suffix
        // };
        // var created = await client.EmergencyVerifications().CreateAsync(verification);
        // Console.WriteLine($"Created verification {created.Data.Id} (status: {created.Data.Status})");

        Console.WriteLine("\nAvailable statuses: " +
            string.Join(", ", Enum.GetNames<EmergencyVerificationStatus>()));
    }
}
