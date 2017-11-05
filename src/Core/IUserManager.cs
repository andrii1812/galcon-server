using System.Threading.Tasks;

namespace Galcon.Server.Core
{
    public interface IUserManager
    {
        int UserCount {get;}
        Task Broadcast(string message);

        Task SendMessage(User user, string message);
    }
}