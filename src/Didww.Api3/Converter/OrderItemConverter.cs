using Didww.Api3.Resource.OrderItem;

namespace Didww.Api3.Converter;

public class OrderItemConverter : PolymorphicListConverter<OrderItemBase>
{
    protected override Dictionary<string, Type> TypeMap { get; } = new()
    {
        ["did_order_items"] = typeof(DidOrderItem),
        ["capacity_order_items"] = typeof(CapacityOrderItem),
        ["generic_order_items"] = typeof(GenericOrderItem),
    };

    protected override string TypeName => "order item";

    protected override string GetItemType(OrderItemBase item) => item.ItemType;
}
