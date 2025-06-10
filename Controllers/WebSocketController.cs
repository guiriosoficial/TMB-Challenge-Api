using Microsoft.AspNetCore.Mvc;
using OrderApi.Services.Interfaces;

namespace OrderApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class WebSocketController : ControllerBase
  {
    private readonly IWebSocketService _webSocketService;

    public WebSocketController(IWebSocketService webSocketService)
    {
      _webSocketService = webSocketService;
    }

    [HttpGet("/ws")]
    public async Task ConnectWebSocket()
    {
      if (HttpContext.WebSockets.IsWebSocketRequest)
      {
        var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        await _webSocketService.HandleWebSocketAsync(webSocket);
      }
      else
      {
        HttpContext.Response.StatusCode = 400;
      }
    }
  }
}