using System;
using System.Globalization;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace GalconServer.Core
{
    public class WebsocketAcceptMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ConnectionManager connectionManager;
        private readonly ITaskHandler handler;
        private readonly IOptions<Configuration> options;

        public WebsocketAcceptMiddleware(
            RequestDelegate next, 
            ConnectionManager connectionManager, 
            ITaskHandler handler, 
            IOptions<Configuration> options)
        {
            this.options = options;
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
                        await context.Response.WriteAsync("User already exists");
                    }
                    else
                    {
                        var userName = userCollection[0];
                        var id = User.CreateId();
                        var user = new User(id, userName);
                        await connectionManager.Add(user, webSocket, handler, options);
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