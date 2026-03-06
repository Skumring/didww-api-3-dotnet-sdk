namespace Didww.Api3.Http;

public class ApiResponse<T>
{
    public T Data { get; }
    public Dictionary<string, object>? Meta { get; }

    public ApiResponse(T data, Dictionary<string, object>? meta = null)
    {
        Data = data;
        Meta = meta;
    }
}
