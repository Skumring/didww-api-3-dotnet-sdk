namespace Didww.Api3.Examples;

public static class BalanceExample
{
    public static async Task RunAsync(DidwwClient client)
    {
        Console.WriteLine("--- Check Balance ---");
        var response = await client.Balance().FindAsync();
        var balance = response.Data;
        Console.WriteLine($"  Balance: {balance.BalanceAmount}");
        Console.WriteLine($"  Credit: {balance.Credit}");
        Console.WriteLine($"  Total: {balance.TotalBalance}");
    }
}
