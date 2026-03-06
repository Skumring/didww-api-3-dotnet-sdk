namespace Didww.Api3.Exception;

public class DidwwClientException : System.Exception
{
    public DidwwClientException(string message) : base(message) { }
    public DidwwClientException(string message, System.Exception innerException) : base(message, innerException) { }
}
