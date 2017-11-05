using System.Threading.Tasks;

namespace GalconServer.Core
{
    public interface IUserManager
    {
        int UserCount {get;}
        Task Broadcast(string message);

        Task SendMessage(User user, string message);
    }
}