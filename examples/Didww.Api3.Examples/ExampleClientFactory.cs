namespace Didww.Api3.Examples;

public static class ExampleClientFactory
{
    public static DidwwClient Create()
    {
        var apiKey = Environment.GetEnvironmentVariable("DIDWW_API_KEY")
                     ?? throw new InvalidOperationException("DIDWW_API_KEY environment variable is not set");

        return DidwwClient.NewBuilder()
            .SetCredentials(new DidwwCredentials(apiKey, DidwwEnvironment.Sandbox))
            .SetTimeout(TimeSpan.FromSeconds(30))
            .Build();
    }
}
