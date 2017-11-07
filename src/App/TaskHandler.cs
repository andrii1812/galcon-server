using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalconServer.App.Events;
using GalconServer.Core;
using GalconServer.Model;
using Newtonsoft.Json.Linq;

namespace GalconServer.App
{
    public class TaskHandler : ITaskHandler, IDisposable
    {
        private readonly ConnectionManager connectionManager;
        private readonly ISerializeManager serializeManager;
        private readonly AGameManager gameManager;
        public TaskHandler(ConnectionManager userManager, ISerializeManager serializeManager, AGameManager gameManager)
        {
            this.gameManager = gameManager;
            this.serializeManager = serializeManager;
            this.connectionManager = userManager;

            gameManager.GameStarted += OnGameStarted;
            gameManager.TickUpdated += OnTickUpdated;
            gameManager.GameOver += OnGameOver;
        }

        private async void OnGameOver(GameOverEventArgs e)
        {
            var data = new EndGameResponse(e.Winner.Id, e.Reason);

            var res = new Message(e.TickID, data, MessageType.EndGame, SenderType.Server, -1);
            var stringResponse = serializeManager.Serialize(res);

            await connectionManager.Broadcast(stringResponse);
        }

        private async void OnTickUpdated(TickUpdateEventArgs e)
        {
            var update = new TickUpdate(e.PlanetUpdate, e.Flights);
            var res = new Message(e.TickID, update, MessageType.TickUpdate, SenderType.Server, -1);
            var stringResponse = serializeManager.Serialize(res);
            
            await connectionManager.Broadcast(stringResponse);
        }

        private async void OnGameStarted(GameStartedEventArgs e)
        {
            var mapResponse = new StartGameResponse(e.Map.Planets, e.Players);
            var res = new Message(e.TickID, mapResponse, MessageType.StartGameResponse, SenderType.Server, -1);
            var stringResponse = serializeManager.Serialize(res);
            
            await connectionManager.Broadcast(stringResponse);
        }

        public async Task Handle(User user, string message)
        {
            var container = serializeManager.Deserialize(message);

            //here go deciding how to handle each request
            switch (container.MessageType)
            {
                case MessageType.StartGame:
                    user.IsReady = true; 

                    if (connectionManager.IsUsersReady) 
                    {
                        var users = connectionManager.Users.ToList();
                        gameManager.StartGame(users[0], users[1]);
                    }     
                    break;
                case MessageType.SendFleet:
                    var data = (container.Data as JToken).ToObject<List<SendFleetCommand>>();

                    var result = gameManager.SendFleet(user.Id, data);

                    var res = new Message(gameManager.TickID, result, MessageType.SendFleetResponse, SenderType.Server, -1);
                    var response = serializeManager.Serialize(res);
                    await connectionManager.Send(user, response);
                    break;
            }
        }

        public async Task UserConnected(User user)
        {
            var data = new ConnectionResponse(user.Id);
            var response = SerializeData(user, MessageType.ConnectionResponse, data);

            await connectionManager.Send(user, response);
        }

        private string SerializeData(User sender, MessageType type, object data)
        {
            var res = new Message(gameManager.TickID, data, type, SenderType.Server, sender?.Id ?? -1);
            return serializeManager.Serialize(res);
        }
        
        public void Dispose()
        {
            gameManager.GameStarted -= OnGameStarted;
            gameManager.TickUpdated -= OnTickUpdated;
        }

        public void UserDisconnected(User user)
        {
            gameManager.PlayerLeft(user);
        }
    }
}