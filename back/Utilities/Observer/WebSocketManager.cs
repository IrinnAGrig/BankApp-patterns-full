using System.Net.WebSockets;
using System.Text;

namespace Backend_BankingApp.Utilities.Observer
{
     public interface ISubject
     {
          Task Attach(string userId, WebSocket socket);
          void Detach(string userId);
          Task Notify(string userId, string message);
     }

     public class WebSocketManager : ISubject
     {
          private readonly Dictionary<string, WebSocket> _sockets = new Dictionary<string, WebSocket>();

          public async Task Attach(string userId, WebSocket socket)
          {
               if (_sockets.TryGetValue(userId, out var existingSocket))
               {
                    if (existingSocket.State == WebSocketState.Open)
                    {
                         await existingSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Replaced by new connection", CancellationToken.None);
                    }
                    _sockets.Remove(userId);
               }

               _sockets[userId] = socket;
          }

          public void Detach(string userId)
          {
               if (_sockets.ContainsKey(userId))
               {
                    _sockets.Remove(userId);
               }
          }

          public async Task Notify(string userId, string message)
          {
               if (_sockets.TryGetValue(userId, out var socket) && socket.State == WebSocketState.Open)
               {
                    var messageBytes = Encoding.UTF8.GetBytes(message);
                    await socket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
               }
          }
     }
}