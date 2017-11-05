namespace Galcon.Server.Core
{
    public interface ISerializeManager
    {
        string Serialize(Container message);

        Container Deserialize(string message);
    }
}