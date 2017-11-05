using System;
using System.Threading.Tasks;
using GalconServer.Core;
using GalconServer.Model;

namespace GalconServer.App
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
            var res = new Message(0, data, type, SenderType.Server, sender?.Id ?? -1);
            return serializeManager.Serialize(res);
        }
    }
}