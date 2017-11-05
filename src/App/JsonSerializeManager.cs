using Galcon.Server.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Galcon.Server.App
{
    public class JsonSerializeManager : ISerializeManager
    {
        public Container Deserialize(string message)
        {
            return JsonConvert.DeserializeObject<Container>(
                message, 
                new JsonSerializerSettings 
                { 
                    ContractResolver = new CamelCasePropertyNamesContractResolver() 
                });
        }

        public string Serialize(Container message)
        {
            return JsonConvert.SerializeObject(
                message, 
                new JsonSerializerSettings 
                { 
                    ContractResolver = new CamelCasePropertyNamesContractResolver() 
                });
        }
    }
}