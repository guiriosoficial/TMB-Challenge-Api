using System.Net.WebSockets;

namespace OrderApi.Services.Interfaces
{
    public interface IWebSocketService
    {
        Task HandleWebSocketAsync(WebSocket webSocket);
        Task SendMessageAsync<T>(T message);
    }
}