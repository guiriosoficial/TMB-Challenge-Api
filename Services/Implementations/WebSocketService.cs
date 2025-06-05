using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using OrderApi.Services.Interfaces;

namespace OrderApi.Services.Implementations
{
    public class WebSocketHandler : IWebSocketHandler
    {
        private readonly ConcurrentDictionary<WebSocket, bool> _sockets = new ConcurrentDictionary<WebSocket, bool>();

        public async Task HandleWebSocketAsync(WebSocket webSocket)
        {
            _sockets.TryAdd(webSocket, true);

            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            _sockets.TryRemove(webSocket, out _);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        public async Task SendMessageAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var tasks = _sockets.Keys.Select(async socket =>
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            });

            await Task.WhenAll(tasks);
        }
    }
}