using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Meeting.Persistence.Infrastructure;

public class PrivateResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(
        MemberInfo memberInfo,
        MemberSerialization memberSerialization)
    {
        JsonProperty prop = base.CreateProperty(
            memberInfo, 
            memberSerialization);

        if (!prop.Writable) 
        {
            var property = memberInfo as PropertyInfo;

            bool hasPrivateSetter = property?.GetSetMethod(true) != null;

            prop.Writable = hasPrivateSetter;
        }

        return prop;
    }
}
