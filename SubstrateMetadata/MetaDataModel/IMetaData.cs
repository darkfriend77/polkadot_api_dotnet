using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SubstrateMetadata
{
    public interface IMetaData
    {
        virtual string Serialize()
        {
            return JsonConvert.SerializeObject(this, new StringEnumConverter());
        }
    }
}