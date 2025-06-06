using System.Net.WebSockets;
using System.Threading.Tasks;

namespace OrderApi.Services.Interfaces
{
    public interface IWebSocketHandler
    {
        Task HandleWebSocketAsync(WebSocket webSocket);
        Task SendMessageAsync<T>(T message);
    }
}