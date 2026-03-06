namespace Didww.Api3;

public class DidwwCredentials
{
    public string ApiKey { get; }
    public DidwwEnvironment Environment { get; }

    public DidwwCredentials(string apiKey, DidwwEnvironment environment)
    {
        ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        Environment = environment;
    }

    public string GetBaseUrl() => Environment.GetBaseUrl();
}
