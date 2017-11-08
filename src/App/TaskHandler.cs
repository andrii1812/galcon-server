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
        private readonly ConnectionManager _connectionManager;
        private readonly ISerializeManager _serializeManager;
        private readonly AGameManager _gameManager;
        public TaskHandler(ConnectionManager connectionManager, ISerializeManager serializeManager, AGameManager gameManager)
        {
            _gameManager = gameManager;
            _serializeManager = serializeManager;
            _connectionManager = connectionManager;

            gameManager.GameStarted += OnGameStarted;
            gameManager.TickUpdated += OnTickUpdated;
            gameManager.GameOver += OnGameOver;
        }

        private async void OnGameOver(GameOverEventArgs e)
        {
            var data = new EndGameResponse(e.Winner.Id, e.Reason);

            var res = new Message(e.TickID, data, MessageType.EndGame, SenderType.Server, -1);
            var stringResponse = _serializeManager.Serialize(res);

            await _connectionManager.Broadcast(stringResponse);
            await _connectionManager.CloseAll();
        }

        private async void OnTickUpdated(TickUpdateEventArgs e)
        {
            var update = new TickUpdate(e.PlanetUpdate, e.Flights);
            var res = new Message(e.TickID, update, MessageType.TickUpdate, SenderType.Server, -1);
            var stringResponse = _serializeManager.Serialize(res);
            
            await _connectionManager.Broadcast(stringResponse);
        }

        private async void OnGameStarted(GameStartedEventArgs e)
        {
            var mapResponse = new StartGameResponse(e.Map.Planets, e.Players);
            var res = new Message(e.TickID, mapResponse, MessageType.StartGameResponse, SenderType.Server, -1);
            var stringResponse = _serializeManager.Serialize(res);
            
            await _connectionManager.Broadcast(stringResponse);
        }

        public async Task Handle(User user, string message)
        {
            var container = _serializeManager.Deserialize(message);

            //here go deciding how to handle each request
            switch (container.MessageType)
            {
                case MessageType.StartGame:
                    user.IsReady = true; 

                    if (_connectionManager.IsUsersReady) 
                    {
                        var users = _connectionManager.Users.ToList();
                        _gameManager.StartGame(users[0], users[1]);
                    }     
                    break;
                case MessageType.SendFleet:
                    var data = (container.Data as JToken).ToObject<List<SendFleetCommand>>();

                    var result = _gameManager.SendFleet(user.Id, data);

                    var res = new Message(_gameManager.TickID, result, MessageType.SendFleetResponse, SenderType.Server, -1);
                    var response = _serializeManager.Serialize(res);
                    await _connectionManager.Send(user, response);
                    break;
            }
        }

        public async Task UserConnected(User user)
        {
            var data = new ConnectionResponse(user.Id);
            var response = SerializeData(user, MessageType.ConnectionResponse, data);

            await _connectionManager.Send(user, response);
        }

        private string SerializeData(User sender, MessageType type, object data)
        {
            var res = new Message(_gameManager.TickID, data, type, SenderType.Server, sender?.Id ?? -1);
            return _serializeManager.Serialize(res);
        }
        
        public void Dispose()
        {
            _gameManager.GameStarted -= OnGameStarted;
            _gameManager.TickUpdated -= OnTickUpdated;
        }

        public void UserDisconnected(User user)
        {
            _connectionManager.RemoveUser(user);
            
            if(!_gameManager.IsGameEnded)
            {
                _gameManager.PlayerLeft(user);
            }
        }
    }
}