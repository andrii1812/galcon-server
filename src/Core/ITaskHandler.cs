using System.Threading.Tasks;

namespace GalconServer.Core
{
    public interface ITaskHandler
    {
        Task Handle(User user, string message);
        Task UserConnected(User user);
    }
}