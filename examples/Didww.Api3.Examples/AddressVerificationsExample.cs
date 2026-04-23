using Didww.Api3.Http;
using Didww.Api3.Resource.Enums;

namespace Didww.Api3.Examples;

// Lists Address Verifications (with 2026-04-16 reject_comment / external_reference_id).
//
// AddressVerification ties an address to one or more DIDs and a set of
// supporting documents so DIDWW compliance can approve or reject the
// declaration. 2026-04-16 adds:
//   * reject_comment        — free-form comment accompanying a rejection
//   * external_reference_id — customer-supplied reference (max 100 chars)
public static class AddressVerificationsExample
{
    public static async Task RunAsync(DidwwClient client)
    {
        Console.WriteLine("=== Address Verifications ===");
        var queryParams = new QueryParams().Include("address", "dids");
        var response = await client.AddressVerifications().ListAsync(queryParams);
        var verifications = response.Data;
        Console.WriteLine($"Found {verifications.Count} address verifications");

        foreach (var av in verifications.Take(5))
        {
            Console.WriteLine($"\nVerification: {av.Id}");
            Console.WriteLine($"  Reference: {av.Reference}");
            Console.WriteLine($"  Status: {av.Status}");
            if (av.ExternalReferenceId != null)
                Console.WriteLine($"  External Reference: {av.ExternalReferenceId}");
            if (av.ServiceDescription != null)
                Console.WriteLine($"  Service description: {av.ServiceDescription}");
            Console.WriteLine($"  Address: {av.Address?.Id}");
            if (av.Dids != null && av.Dids.Count > 0)
                Console.WriteLine($"  DIDs: {string.Join(", ", av.Dids.Select(d => d.Number))}");
            if (av.IsRejected)
            {
                if (av.RejectReasons != null)
                    Console.WriteLine($"  Reject reasons: {string.Join(", ", av.RejectReasons)}");
                if (av.RejectComment != null)
                    Console.WriteLine($"  Reject comment: {av.RejectComment}");
            }
        }

        // Filter: only rejected verifications
        Console.WriteLine("\n=== Rejected verifications ===");
        var rejectedParams = new QueryParams().Filter("status", "rejected");
        var rejected = await client.AddressVerifications().ListAsync(rejectedParams);
        Console.WriteLine($"Found {rejected.Data.Count} rejected verifications");
        foreach (var av in rejected.Data.Take(3))
        {
            var reason = av.RejectComment ?? (av.RejectReasons != null ? string.Join(", ", av.RejectReasons) : "");
            Console.WriteLine($"  {av.Reference}: {reason}");
        }

        Console.WriteLine("\nAvailable statuses: " +
            string.Join(", ", Enum.GetNames<AddressVerificationStatus>()));
    }
}
