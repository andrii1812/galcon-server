using GalconServer.Core;
using GalconServer.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GalconServer.App
{
    public class JsonSerializeManager : ISerializeManager
    {
        public Message Deserialize(string message)
        {
            return JsonConvert.DeserializeObject<Message>(
                message, 
                new JsonSerializerSettings 
                { 
                    ContractResolver = new CamelCasePropertyNamesContractResolver() 
                });
        }

        public string Serialize(Message message)
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