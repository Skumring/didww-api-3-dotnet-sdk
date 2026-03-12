using System.Reflection;
using System.Text;
using Didww.Api3.Converter;
using Didww.Api3.Exception;
using Didww.Api3.Http;
using Didww.Api3.Resource;
using JsonApiSerializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Didww.Api3.Repository;

public class Repository<T> : ReadOnlyRepository<T> where T : BaseResource
{
    private const string JsonApiMediaType = "application/vnd.api+json";

    public Repository(HttpClient httpClient, JsonSerializerSettings serializerSettings,
        string baseUrl, string endpoint)
        : base(httpClient, serializerSettings, baseUrl, endpoint)
    {
    }

    public async Task<ApiResponse<T>> CreateAsync(T resource, QueryParams? queryParams = null)
    {
        var url = BaseUrl + "/" + Endpoint + (queryParams?.ToQueryString() ?? "");
        var payload = JsonConvert.SerializeObject(resource, SerializerSettings);
        var content = new StringContent(payload, Encoding.UTF8, JsonApiMediaType);
        var response = await HttpClient.PostAsync(url, content);
        await HandleErrorResponseAsync(response);
        var body = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<T>(body, SerializerSettings)!;
        EnableDirtyTracking(data);
        return new ApiResponse<T>(data, ExtractMeta(body));
    }

    public async Task<ApiResponse<T>> UpdateAsync(T resource, QueryParams? queryParams = null)
    {
        var id = resource.Id ?? throw new DidwwClientException("Resource ID is null");
        var url = BaseUrl + "/" + Endpoint + "/" + id + (queryParams?.ToQueryString() ?? "");

        string payload;
        try
        {
            DirtySerializationContext.EnableDirtyOnlyMode();
            payload = JsonConvert.SerializeObject(resource, SerializerSettings);
            payload = EnsureDirtyNullRelationships(resource, payload);
        }
        finally
        {
            DirtySerializationContext.DisableDirtyOnlyMode();
        }

        var content = new StringContent(payload, Encoding.UTF8, JsonApiMediaType);
        var request = new HttpRequestMessage(new HttpMethod("PATCH"), url) { Content = content };
        var response = await HttpClient.SendAsync(request);
        await HandleErrorResponseAsync(response);
        var body = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<T>(body, SerializerSettings)!;
        EnableDirtyTracking(data);
        return new ApiResponse<T>(data, ExtractMeta(body));
    }

    public async Task DeleteAsync(string id)
    {
        var url = BaseUrl + "/" + Endpoint + "/" + id;
        var response = await HttpClient.DeleteAsync(url);
        await HandleErrorResponseAsync(response);
    }

    private string EnsureDirtyNullRelationships(T resource, string payload)
    {
        try
        {
            var rootNode = JObject.Parse(payload);
            if (rootNode["data"] is not JObject dataNode)
                return payload;

            var relationshipsNode = dataNode["relationships"] as JObject ?? new JObject();

            ForEachDeclaredProperty(resource, prop =>
            {
                if (!IsRelationshipProperty(prop))
                    return;

                var relName = GetJsonPropertyName(prop);
                if (!resource.IsFieldDirty(relName) || prop.GetValue(resource) != null)
                    return;

                relationshipsNode[relName] = new JObject
                {
                    ["data"] = IsListRelationship(prop) ? (JToken)new JArray() : JValue.CreateNull()
                };
            });

            dataNode["relationships"] = relationshipsNode;
            return rootNode.ToString(Formatting.None);
        }
        catch (System.Exception)
        {
            return payload;
        }
    }

    private static bool IsRelationshipProperty(PropertyInfo prop) =>
        typeof(BaseResource).IsAssignableFrom(prop.PropertyType) || IsListRelationship(prop);

    private static bool IsListRelationship(PropertyInfo prop) =>
        prop.PropertyType.IsGenericType &&
        prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>) &&
        typeof(BaseResource).IsAssignableFrom(prop.PropertyType.GetGenericArguments()[0]);

    private static string GetJsonPropertyName(PropertyInfo prop) =>
        prop.GetCustomAttributes(typeof(JsonPropertyAttribute), false)
            .OfType<JsonPropertyAttribute>()
            .FirstOrDefault()?.PropertyName ?? prop.Name;
}
