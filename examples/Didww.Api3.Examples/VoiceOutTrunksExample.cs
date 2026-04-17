using Didww.Api3.Resource;
using Didww.Api3.Resource.Configuration.AuthenticationMethod;
using Didww.Api3.Resource.Enums;

namespace Didww.Api3.Examples;

public static class VoiceOutTrunksExample
{
    public static async Task RunAsync(DidwwClient client)
    {
        var suffix = Guid.NewGuid().ToString()[..8];

        Console.WriteLine("--- Create Voice Out Trunk ---");
        var trunk = new VoiceOutTrunk
        {
            Name = "My Outbound Trunk " + suffix,
            DefaultDstAction = DefaultDstAction.AllowAll,
            OnCliMismatchAction = OnCliMismatchAction.RejectCall,
            AuthenticationMethod = new IpOnlyAuthenticationMethod
            {
                // Replace with your real infrastructure CIDR
                AllowedSipIps = new List<string> { "203.0.113.0/24" },
                TechPrefix = ""
            }
        };

        var response = await client.VoiceOutTrunks().CreateAsync(trunk);
        var created = response.Data;
        Console.WriteLine($"  Created: {created.Id}");
        Console.WriteLine($"    Name: {created.Name}");
        if (created.AuthenticationMethod is CredentialsAndIpAuthenticationMethod cai)
        {
            Console.WriteLine($"    Username: {cai.Username}");
            Console.WriteLine($"    Password: {cai.Password}");
        }
        Console.WriteLine($"    Status: {created.Status}");

        Console.WriteLine("\n--- List Voice Out Trunks ---");
        var listResponse = await client.VoiceOutTrunks().ListAsync();
        Console.WriteLine($"  Total: {listResponse.Data.Count}");
        foreach (var t in listResponse.Data.Take(5))
        {
            Console.WriteLine($"    {t.Name} ({t.Status})");
        }

        Console.WriteLine("\n--- Update Voice Out Trunk ---");
        created.Name = "Updated Outbound Trunk " + suffix;
        created.AuthenticationMethod = new IpOnlyAuthenticationMethod
        {
            AllowedSipIps = new List<string> { "10.0.0.0/8" },
            TechPrefix = ""
        };
        var updated = (await client.VoiceOutTrunks().UpdateAsync(created)).Data;
        Console.WriteLine($"  Updated name: {updated.Name}");

        Console.WriteLine("\n--- Delete Voice Out Trunk ---");
        await client.VoiceOutTrunks().DeleteAsync(created.Id!);
        Console.WriteLine($"  Deleted: {created.Id}");
    }
}
