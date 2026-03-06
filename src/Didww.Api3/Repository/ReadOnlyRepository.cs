using Didww.Api3.Converter;
using Didww.Api3.Exception;
using Didww.Api3.Http;
using Didww.Api3.Resource;
using JsonApiSerializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Didww.Api3.Repository;

public class ReadOnlyRepository<T> where T : BaseResource
{
    protected readonly HttpClient HttpClient;
    protected readonly JsonSerializerSettings SerializerSettings;
    protected readonly string BaseUrl;
    protected readonly string Endpoint;

    public ReadOnlyRepository(HttpClient httpClient, JsonSerializerSettings serializerSettings,
        string baseUrl, string endpoint)
    {
        HttpClient = httpClient;
        SerializerSettings = serializerSettings;
        BaseUrl = baseUrl;
        Endpoint = endpoint;
    }

    public async Task<ApiResponse<List<T>>> ListAsync(QueryParams? queryParams = null)
    {
        var url = BaseUrl + "/" + Endpoint + (queryParams?.ToQueryString() ?? "");
        var response = await HttpClient.GetAsync(url);
        await HandleErrorResponseAsync(response);
        var body = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<List<T>>(body, SerializerSettings) ?? new List<T>();
        var meta = ExtractMeta(body);
        EnableDirtyTracking(data);
        return new ApiResponse<List<T>>(data, meta);
    }

    public async Task<ApiResponse<T>> FindAsync(string id, QueryParams? queryParams = null)
    {
        var url = BaseUrl + "/" + Endpoint + "/" + id + (queryParams?.ToQueryString() ?? "");
        var response = await HttpClient.GetAsync(url);
        await HandleErrorResponseAsync(response);
        var body = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<T>(body, SerializerSettings)!;
        var meta = ExtractMeta(body);
        EnableDirtyTracking(data);
        return new ApiResponse<T>(data, meta);
    }

    protected async Task HandleErrorResponseAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return;

        var body = await response.Content.ReadAsStringAsync();
        var errors = new List<ApiError>();
        try
        {
            var root = JObject.Parse(body);
            var errorsNode = root["errors"] as JArray;
            if (errorsNode != null)
            {
                foreach (var errorNode in errorsNode)
                {
                    errors.Add(errorNode.ToObject<ApiError>()!);
                }
            }
        }
        catch
        {
            // ignore parse errors
        }

        if (errors.Count == 0)
            throw new DidwwApiException((int)response.StatusCode, body);

        throw new DidwwApiException((int)response.StatusCode, errors);
    }

    protected Dictionary<string, object>? ExtractMeta(string body)
    {
        try
        {
            var root = JObject.Parse(body);
            var meta = root["meta"];
            if (meta != null)
                return meta.ToObject<Dictionary<string, object>>();
        }
        catch
        {
            // ignore
        }
        return null;
    }

    protected void EnableDirtyTracking(T resource)
    {
        if (resource is BaseResource br)
            br.EnableDirtyTracking();
    }

    protected void EnableDirtyTracking(List<T> resources)
    {
        foreach (var resource in resources)
        {
            if (resource is BaseResource br)
                br.EnableDirtyTracking();
        }
    }
}
