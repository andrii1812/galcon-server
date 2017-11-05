using System;
using System.Threading.Tasks;
using Galcon.Server.Core;

namespace Galcon.Server.App
{
    public class TaskHandler : ITaskHandler
    {
        private readonly IUserManager userManager;
        private readonly ISerializeManager serializeManager;
        public TaskHandler(IUserManager userManager, ISerializeManager serializeManager)
        {
            this.serializeManager = serializeManager;
            this.userManager = userManager;
        }

        public async Task Handle(User user, string message)
        {
            var container = serializeManager.Deserialize(message);

            //here go deciding how to handle each request
            switch(container.MessageType) 
            {
                
            }

            await userManager.Broadcast(user.Name + ": " + message);
        }

        public async Task UserConnected(User user)
        {
            var data = new { PlayerId = user.Id };
            var response = SerializeData(user, MessageType.ConnectionResponse, data);

            await userManager.SendMessage(user, response);
        }

        private string SerializeData(User sender, MessageType type, object data)
        {
            var res = new Container
            {
                TickId = 0, // TODO: add tick Id
                Data = data,
                MessageType = type,
                Sender = sender?.Id ?? -1
            };
            return serializeManager.Serialize(res);
        }
    }
}