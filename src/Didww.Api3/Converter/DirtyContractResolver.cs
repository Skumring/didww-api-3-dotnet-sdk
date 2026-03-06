using Didww.Api3.Resource;
using JsonApiSerializer.ContractResolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Didww.Api3.Converter;

public class DirtyContractResolver : JsonApiContractResolver
{
    protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (typeof(BaseResource).IsAssignableFrom(property.DeclaringType))
        {
            var baseShouldSerialize = property.ShouldSerialize;
            property.ShouldSerialize = instance =>
            {
                // Let base decide first
                if (baseShouldSerialize != null && !baseShouldSerialize(instance))
                    return false;

                if (instance is not BaseResource resource)
                    return true;

                if (!DirtySerializationContext.IsDirtyOnlyModeEnabled)
                    return true;

                // In dirty-only mode, only serialize id + type + dirty fields
                if (property.PropertyName == "id" || property.PropertyName == "type")
                    return true;

                return resource.IsFieldDirty(property.PropertyName!);
            };
        }

        return property;
    }
}
