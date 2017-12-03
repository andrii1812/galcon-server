using GalconServer.App;
using GalconServer.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalconServer.Core.Tests
{
    [TestClass]
    public class JsonSerializeManagerTests
    {
        private JsonSerializeManager sut;

        [TestMethod]
        public void Serialize_ShouldCorectlySerializeMessageObject()
        {
            sut = new JsonSerializeManager();
            var message = new Message(1, new {CamelCaseKey="value"}, MessageType.StartGame, SenderType.Server, 12);

            var result = sut.Serialize(message);

            Assert.AreEqual(result, "{\"tickId\":1,\"data\":{\"camelCaseKey\":\"value\"},\"messageType\":1,\"senderType\":1,\"sender\":12}");
        }

        [TestMethod]
        public void Deserialize_ShouldCorectlyDeserializeMessageObject()
        {
            sut = new JsonSerializeManager();
            var input = "{\"tickId\":1,\"data\":null,\"messageType\":1,\"senderType\":1,\"sender\":12}";

            var result = sut.Deserialize(input);

            Assert.AreEqual(result.MessageType, MessageType.StartGame);
            Assert.AreEqual(result.SenderType, SenderType.Server);
            Assert.AreEqual(result.TickId, 1);
            Assert.AreEqual(result.Sender, 12);
            Assert.AreEqual(result.Data, null);
        }
    }
}