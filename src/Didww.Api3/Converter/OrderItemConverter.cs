using Didww.Api3.Resource.OrderItem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Didww.Api3.Converter;

public class OrderItemConverter : JsonConverter<List<OrderItemBase>>
{
    private static readonly Dictionary<string, Type> TypeMap = new()
    {
        ["did_order_items"] = typeof(DidOrderItem),
        ["capacity_order_items"] = typeof(CapacityOrderItem),
        ["generic_order_items"] = typeof(GenericOrderItem),
        ["emergency_order_items"] = typeof(EmergencyOrderItem),
    };

    public override List<OrderItemBase>? ReadJson(JsonReader reader, Type objectType, List<OrderItemBase>? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);
        if (token.Type == JTokenType.Null)
            return null;

        var items = new List<OrderItemBase>();
        if (token.Type == JTokenType.Array)
        {
            foreach (var itemNode in token)
            {
                var item = PolymorphicJsonHelper.Deserialize<OrderItemBase>(itemNode, TypeMap, "order item", serializer);
                if (item != null)
                    items.Add(item);
            }
        }
        return items;
    }

    public override void WriteJson(JsonWriter writer, List<OrderItemBase>? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartArray();
        foreach (var item in value)
            PolymorphicJsonHelper.Serialize(item, item.ItemType, serializer).WriteTo(writer);
        writer.WriteEndArray();
    }
}
