using System.Threading.Tasks;
using Galcon.Server.Core;

namespace Galcon.Server.App
{
    public class TaskHandler : ITaskHandler
    {
        private readonly IUserManager userManager;
        public TaskHandler(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public async Task Handle(User user, string message)
        {
            await userManager.Broadcast(user.Name + ": " + message);
        }
    }
}