using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Backend_BankingApp.Utilities.Observer
{
     public class WebSocketService
     {
          private readonly WebSocketManager _webSocketManager;

          public WebSocketService(WebSocketManager webSocketManager)
          {
               _webSocketManager = webSocketManager;
          }

          public async Task SendMessageToClient(string userId, string message)
          {
               try
               {
                    await _webSocketManager.Notify(userId, message);
                    Console.WriteLine($"Message sent to user {userId}: {message}");
               }
               catch (Exception ex)
               {
                    Console.WriteLine($"Failed to send message to user {userId}: {ex.Message}");
               }
          }

     }

     public interface IWebSocketHandler
     {
          Task HandleAsync(CancellationToken cancellationToken);
          Task SendMessageAsync(string message, CancellationToken cancellationToken);
     }
     public class WebSocketHandler : IWebSocketHandler
     {
          private readonly WebSocket _webSocket;
          private readonly string _userId;
          private readonly WebSocketManager _webSocketManager;

          public WebSocketHandler(WebSocket webSocket, string userId, WebSocketManager webSocketManager)
          {
               _webSocket = webSocket;
               _userId = userId;
               _webSocketManager = webSocketManager;
          }

          public async Task HandleAsync(CancellationToken cancellationToken)
          {
               var buffer = new byte[1024 * 4];
               while (_webSocket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
               {
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                         await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", cancellationToken);
                         _webSocketManager.Detach(_userId);
                    }
                    else if (result.MessageType == WebSocketMessageType.Text)
                    {
                         var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                         Console.WriteLine($"Received JSON: {receivedMessage}");

                         var processedData = ProcessMessage(receivedMessage);

                         var responseMessage = JsonSerializer.Serialize(processedData);
                         await SendMessageAsync(responseMessage, cancellationToken);
                    }
               }
          }

          private object ProcessMessage(string jsonMessage)
          {
               try
               {
                    var data = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonMessage);

                    if (data != null && data.ContainsKey("action"))
                    {
                         return new
                         {
                              status = "success",
                              message = $"Action '{data["action"]}' was processed successfully."
                         };
                    }
                    else
                    {
                         return new
                         {
                              status = "error",
                              message = "Invalid JSON structure."
                         };
                    }
               }
               catch (Exception ex)
               {
                    return new
                    {
                         status = "error",
                         message = "Failed to process JSON: " + ex.Message
                    };
               }
          }

          public async Task SendMessageAsync(string message, CancellationToken cancellationToken)
          {
               if (_webSocket.State == WebSocketState.Open)
               {
                    var messageBytes = Encoding.UTF8.GetBytes(message);
                    await _webSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, cancellationToken);
               }
          }

     }
}
