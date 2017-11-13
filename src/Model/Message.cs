namespace GalconServer.Model
{
    public class Message
    {
        public int TickId { get; private set; }
        public object Data { get; private set; }
        public MessageType MessageType { get; private set; }
        public SenderType SenderType { get; private set; }
        public int Sender { get; private set; }

        public Message(int tickId, object data, MessageType messageType, SenderType senderType, int sender)
        {
            TickId = tickId;
            Data = data;
            MessageType = messageType;
            SenderType = senderType;
            Sender = sender;
        }
    }
}