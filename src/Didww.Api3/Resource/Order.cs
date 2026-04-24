using Didww.Api3.Converter;
using Didww.Api3.Resource.Enums;
using Didww.Api3.Resource.OrderItem;
using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class Order : BaseResource
{
    public override string Type => "orders";

    public static Order Build(string id) => BaseResource.Build<Order>(id);

    [JsonProperty("amount")]
    public decimal? Amount { get; set; }

    [JsonProperty("status")]
    public OrderStatus? Status { get; set; }

    [JsonIgnore]
    public bool IsPending => Status == OrderStatus.Pending;
    [JsonIgnore]
    public bool IsCompleted => Status == OrderStatus.Completed;
    [JsonIgnore]
    public bool IsCanceled => Status == OrderStatus.Canceled;

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("reference")]
    public string? Reference { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    private string? _callbackUrl;
    [JsonProperty("callback_url")]
    public string? CallbackUrl
    {
        get => _callbackUrl;
        set => SetProperty(ref _callbackUrl, value);
    }

    private CallbackMethod? _callbackMethod;
    [JsonProperty("callback_method")]
    public CallbackMethod? CallbackMethod
    {
        get => _callbackMethod;
        set => SetProperty(ref _callbackMethod, value);
    }

    private bool? _allowBackOrdering;
    [JsonProperty("allow_back_ordering")]
    public bool? AllowBackOrdering
    {
        get => _allowBackOrdering;
        set => SetProperty(ref _allowBackOrdering, value);
    }

    private string? _externalReferenceId;
    [JsonProperty("external_reference_id")]
    public string? ExternalReferenceId { get => _externalReferenceId; set => SetProperty(ref _externalReferenceId, value); }

    private List<OrderItemBase>? _items;
    [JsonProperty("items")]
    [JsonConverter(typeof(OrderItemConverter))]
    public List<OrderItemBase>? Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }
}
