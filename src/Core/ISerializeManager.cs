using GalconServer.Model;

namespace GalconServer.Core
{
    public interface ISerializeManager
    {
        string Serialize(Message message);

        Message Deserialize(string message);
    }
}