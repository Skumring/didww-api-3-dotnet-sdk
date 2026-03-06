using FluentAssertions;

namespace Didww.Api3.Tests;

public class DidwwClientTest
{
    [Fact]
    public void TestProductionBaseUrl()
    {
        var credentials = new DidwwCredentials("test-key", DidwwEnvironment.Production);
        var client = DidwwClient.NewBuilder()
            .SetCredentials(credentials)
            .Build();

        client.BaseUrl.Should().Be("https://api.didww.com/v3");
    }

    [Fact]
    public void TestSandboxBaseUrl()
    {
        var credentials = new DidwwCredentials("test-key", DidwwEnvironment.Sandbox);
        var client = DidwwClient.NewBuilder()
            .SetCredentials(credentials)
            .Build();

        client.BaseUrl.Should().Be("https://sandbox-api.didww.com/v3");
    }

    [Fact]
    public void TestCustomBaseUrl()
    {
        var credentials = new DidwwCredentials("test-key", DidwwEnvironment.Sandbox);
        var client = DidwwClient.NewBuilder()
            .SetCredentials(credentials)
            .SetBaseUrl("http://localhost:8080/v3")
            .Build();

        client.BaseUrl.Should().Be("http://localhost:8080/v3");
    }

    [Fact]
    public void TestBuilderRequiresCredentials()
    {
        var act = () => DidwwClient.NewBuilder().Build();
        act.Should().Throw<ArgumentNullException>();
    }
}
