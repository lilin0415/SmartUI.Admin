using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Koo.Utilities.Messaging;

namespace YiSha.Service.WebSocketManager
{
    public abstract class WebSocketHandler
    {
        protected WebSocketConnectionManager WebSocketConnectionManager
        {
            get; set;
        }

        public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager)
        {
            WebSocketConnectionManager = webSocketConnectionManager;
        }

        public virtual async Task OnConnected(WebSocket socket,string key)
        {
            WebSocketConnectionManager.AddSocket(socket,key);
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await WebSocketConnectionManager.RemoveSocket(WebSocketConnectionManager.GetId(socket));
        }

        public async Task SendMessageAsync(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;

            var bytes = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(buffer: new ArraySegment<byte>(array: bytes, offset: 0, count: message.Length),
                                    messageType: WebSocketMessageType.Text,
                                    endOfMessage: true,
                                    cancellationToken: CancellationToken.None);
        }

        public async Task SendMessageAsync<T>(string socketId, T data)
        {
            var msg = Message.Create(socketId, data);
           // await SendMessageAsync(WebSocketConnectionManager.GetSocketById(socketId), msg);
        }

        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var pair in WebSocketConnectionManager.GetAll())
            {
                if (pair.Value.State == WebSocketState.Open)
                    await SendMessageAsync(pair.Value, message);
            }
        }
        /// <summary>
        /// 给一堆人发消息
        /// </summary>
        /// <param name="webSockets"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToSome(WebSocket[] webSockets, string message)
        {
            foreach (var pair in webSockets)
            {
                await SendMessageAsync(pair, message);
            }
            
        }
        public IEnumerable<WebSocket> GetSomeWebsocket(string[] keys)
        {
            foreach (var key in keys)
            {
                yield return WebSocketConnectionManager.GetWebSocket(key);
            }
        }
        public virtual async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var str = Encoding.UTF8.GetString(buffer);
            await ReceiveAsync(socket, result, str);
        }

        protected virtual async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, string str)
        {
            
        }
    }
}
