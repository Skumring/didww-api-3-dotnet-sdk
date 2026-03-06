namespace Didww.Api3;

public enum DidwwEnvironment
{
    Production,
    Sandbox
}

public static class DidwwEnvironmentExtensions
{
    public static string GetBaseUrl(this DidwwEnvironment environment) => environment switch
    {
        DidwwEnvironment.Production => "https://api.didww.com/v3",
        DidwwEnvironment.Sandbox => "https://sandbox-api.didww.com/v3",
        _ => throw new ArgumentOutOfRangeException(nameof(environment))
    };
}
