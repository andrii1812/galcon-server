using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Galcon.Server.Core
{
    public class UserManager : IUserManager
    {
        private readonly ConnectionManager connectionManager;

        public UserManager(ConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;
        }

        public async Task Broadcast(string message)
        {
            await connectionManager.Broadcast(message);
        }

        public async Task SendMessage(User user, string message)
        {
            await connectionManager.Send(user.Name, message);
        }
    }
}