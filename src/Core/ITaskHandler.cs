using System.Threading.Tasks;

namespace Galcon.Server.Core
{
    public interface ITaskHandler
    {
        Task Handle(User user, string message);
        Task UserConnected(User user);
    }
}