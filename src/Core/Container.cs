namespace Galcon.Server.Core
{
    public class Container
    {
        public int TickId {get; set;}

        public object Data {get;set;}

        public MessageType MessageType {get;set;}

        public int Sender {get;set;}

        public SenderType SenderType {get;set;} = SenderType.Server;
    }
}