using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace GalconServer.Core
{
    public class ConnectionManager
    {
        private ConcurrentDictionary<User, WebSocket> _dictionary = new ConcurrentDictionary<User, WebSocket>();
        private IOptions<Configuration> _options;

        public IEnumerable<User> Users => _dictionary.Keys;

        public bool IsUsersReady => _dictionary.Keys.Count == 2 && _dictionary.Keys.All(x => x.IsReady);

        internal async Task Send(User user, string message)
        {
            WebSocket socket;
            while(!_dictionary.TryGetValue(user, out socket));

            await Send(socket, message);
        }

        internal async Task Broadcast(string message)
        {
            foreach(var sock in _dictionary.Values)
            {
                await Send(sock, message);
            }
        }

        internal async Task Add(User user, WebSocket webSocket, ITaskHandler handler, IOptions<Configuration> options)
        {
            _options = options;
            while(!_dictionary.TryAdd(user, webSocket));
            await handler.UserConnected(user);

            await RecieveLoop(user, webSocket, handler);
        }

        private async Task RecieveLoop(User user, WebSocket webSocket, ITaskHandler handler)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result;
            do
            {
                result = webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).Result;
                var str = System.Text.Encoding.UTF8.GetString(buffer, 0,  result.Count);

                await handler.Handle(user, str);
            } 
            while (!result.CloseStatus.HasValue);

            handler.UserDisconnected(user);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        private async Task Send(WebSocket sock, string message)
        {            
            var bytes = System.Text.Encoding.UTF8.GetBytes(message);

            await sock.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}