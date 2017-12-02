using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GalconServer.Core
{
    public class ConnectionManager
    {
        private ConcurrentDictionary<User, WebSocket> _dictionary = new ConcurrentDictionary<User, WebSocket>();
        private IOptions<Configuration> _options;
        private readonly ILogger<ConnectionManager> _logger;
        private ITaskHandler _handler;

        public IEnumerable<User> Users => _dictionary.Keys;

        public bool IsUsersReady => _dictionary.Keys.Count == 2 && _dictionary.Keys.All(x => x.IsReady);

        public ConnectionManager(IOptions<Configuration> options, ILogger<ConnectionManager> logger)
        {
            _options = options;
            _logger = logger;
        }

        internal async Task Send(User user, string message)
        {
            WebSocket socket;
            while(!_dictionary.TryGetValue(user, out socket));

            await Send(user, socket, message);
        }

        internal void RegisterHandler(ITaskHandler handler)
        {
            _handler = handler;
        }

        internal async Task Broadcast(string message)
        {
            foreach(var pair in _dictionary.AsEnumerable())
            {
                await Send(pair.Key, pair.Value, message);
            }
        }

        internal async Task Add(User user, WebSocket webSocket)
        {
            while(!_dictionary.TryAdd(user, webSocket));
            await _handler.UserConnected(user);

            await ReceiveLoop(user, webSocket);
        }

        internal async Task CloseAll()
        {
            foreach(var sock in _dictionary.Values)
            {
                await TryClose(sock);
            }
        }

        private async Task TryClose(WebSocket sock)
        {
            try
            {
                await sock.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Exception occured while closing socket");
            }
        }

        private async Task ReceiveLoop(User user, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result;
            do
            {
                result = webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).Result;
                var str = System.Text.Encoding.UTF8.GetString(buffer, 0,  result.Count);

                if(!result.CloseStatus.HasValue) 
                {
                    await _handler.Handle(user, str);
                }
            } 
            while (!result.CloseStatus.HasValue);
            
            _handler.UserDisconnected(user);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        private async Task Send(User user, WebSocket sock, string message)
        {            
            var bytes = System.Text.Encoding.UTF8.GetBytes(message);

            try
            {
                await sock.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            } 
            catch(Exception ex)
            {
                if(ex is InvalidOperationException || ex is WebSocketException)
                {
                    _handler.UserDisconnected(user);
                    return;
                }
                _logger.LogError(0, ex, "Unknown exception occured while sending on socket");
                throw;
            }
        }

        internal void RemoveUser(User user)
        {
            WebSocket t;
            while(!_dictionary.TryRemove(user, out t));
        }
    }
}