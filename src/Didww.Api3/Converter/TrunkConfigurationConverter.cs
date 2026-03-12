using Didww.Api3.Resource.Configuration;

namespace Didww.Api3.Converter;

public class TrunkConfigurationConverter : PolymorphicSingleConverter<TrunkConfiguration>
{
    protected override Dictionary<string, Type> TypeMap { get; } = new()
    {
        ["sip_configurations"] = typeof(SipConfiguration),
        ["pstn_configurations"] = typeof(PstnConfiguration),
    };

    protected override string TypeName => "trunk configuration";

    protected override string GetItemType(TrunkConfiguration item) => item.ConfigurationType;
}
