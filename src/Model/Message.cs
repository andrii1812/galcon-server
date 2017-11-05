﻿namespace GalconServer.Model
{
    public class Message
    {
        public int TickID { get; private set; }
        public string Data { get; private set; }
        public MessageType MessageType { get; private set; }
        public SenderType SenderType { get; private set; }
        public int Sender { get; private set; }

        public Message(int tickID, string data, MessageType messageType, SenderType senderType, int sender)
        {
            TickID = tickID;
            Data = data;
            MessageType = messageType;
            SenderType = senderType;
            Sender = sender;
        }
    }
}