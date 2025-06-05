using Microsoft.AspNetCore.Mvc;
using OrderApi.Services.Interfaces;

namespace OrderApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class WebSocketController : ControllerBase
  {
    private readonly IWebSocketHandler _webSocketHandler;

    public WebSocketController(IWebSocketHandler webSocketHandler)
    {
      _webSocketHandler = webSocketHandler;
    }

    [HttpGet("/ws")]
    public async Task ConnectWebSocket()
    {
      if (HttpContext.WebSockets.IsWebSocketRequest)
      {
        var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        await _webSocketHandler.HandleWebSocketAsync(webSocket);
      }
      else
      {
        HttpContext.Response.StatusCode = 400;
      }
    }
  }
}