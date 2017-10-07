using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Galcon.Server.Core
{
    public class ConnectionManager
    {
        
        private ConcurrentDictionary<string, WebSocket> _dictionary = new ConcurrentDictionary<string, WebSocket>();
        private readonly HttpContext context;

        internal async Task Send(string name, string message)
        {
            WebSocket socket;
            while(!_dictionary.TryGetValue(name, out socket));

            await Send(socket, message);
        }

        internal async Task Broadcast(string message)
        {
            foreach(var sock in _dictionary.Values)
            {
                await Send(sock, message);
            }
        }

        internal async Task Add(string user, WebSocket webSocket, ITaskHandler handler)
        {
            _dictionary.TryAdd(user, webSocket);

            await RecieveLoop(user, webSocket, handler);
        }

        private async Task RecieveLoop(string user, WebSocket webSocket, ITaskHandler handler)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result;
            do
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var str = System.Text.Encoding.UTF8.GetString(buffer, 0,  result.Count);

                await handler.Handle(new User {Name=user}, str);
            } 
            while (!result.CloseStatus.HasValue);

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        private async Task Send(WebSocket sock, string message)
        {            
            var bytes = System.Text.Encoding.UTF8.GetBytes(message);

            await sock.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}