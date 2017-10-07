using System;
using System.Globalization;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Galcon.Server.Core
{
    public class WebsocketAcceptMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ConnectionManager connectionManager;
        private readonly ITaskHandler handler;

        public WebsocketAcceptMiddleware(RequestDelegate next, ConnectionManager connectionManager, ITaskHandler handler)
        {
            this.handler = handler;
            this.connectionManager = connectionManager;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                    var userCollection = context.Request.Query["name"];
                    if (userCollection.Count != 1)
                    {
                        context.Response.StatusCode = 400;
                    }
                    else
                    {
                        var user = userCollection[0];
                        await connectionManager.Add(user, webSocket, handler);
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }

            await this._next(context);
        }
    }
}