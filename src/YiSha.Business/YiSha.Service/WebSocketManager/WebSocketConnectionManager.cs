using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace YiSha.Service.WebSocketManager
{
    public class WebSocketConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public int GetCount()
        {
            return _sockets.Count;
        }
        public WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key;
        }
        public WebSocket GetWebSocket(string key)
        {
            WebSocket _socket;
            _sockets.TryGetValue(key, out _socket);
            return _socket;

        }
        public void AddSocket(WebSocket socket)
        {
            _sockets.TryAdd(CreateConnectionId(), socket);
        }
        public void AddSocket(WebSocket socket, string key)
        {
            if (GetWebSocket(key) != null)
            {
                _sockets.TryRemove(key, out WebSocket destoryWebsocket);
            }
            _sockets.TryAdd(key, socket);
            //string sId = CreateConnectionId();
            //while (!_sockets.TryAdd(sId, socket))
            //{
            //    sId = CreateConnectionId();
            //}



        }
        public async Task RemoveSocket(string id)
        {
            WebSocket socket;
            _sockets.TryRemove(id, out socket);

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the ConnectionManager",
                                    cancellationToken: CancellationToken.None);

            //await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
        }
        public async Task CloseSocket(WebSocket socket)
        {
            await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
        }
        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
