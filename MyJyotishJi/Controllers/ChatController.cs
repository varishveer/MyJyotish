using BusinessAccessLayer.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace MyJyotishGApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private static readonly ConcurrentDictionary<string, WebSocket> _clients = new();

        private readonly IChat _chat;

        public ChatController(IChat chat)
        {
            _chat = chat;
        }

        [HttpGet("connect")]
        public async Task Connect(string id, string sendBy)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
               var changeIdPref=sendBy=="client"?id+"A":id+"B";
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _clients[changeIdPref] = webSocket;

                await HandleWebSocketCommunication(webSocket, id, sendBy);
            }
            else
            {
                HttpContext.Response.StatusCode = 400; // Bad Request
            }
        }

        private async Task HandleWebSocketCommunication(WebSocket webSocket, string clientId, string sendBy)
        {
            var buffer = new byte[1024 * 4];
            var pingDelay = TimeSpan.FromSeconds(30);
            var cancellationTokenSource = new CancellationTokenSource();
            try
            {
                var pingTask = Task.Run(async () =>
                {
                    while (webSocket.State == WebSocketState.Open)
                    {
                        await Task.Delay(pingDelay, cancellationTokenSource.Token);

                        if (webSocket.State == WebSocketState.Open)
                        {
                            try
                            {
                                var pingMessage = new ArraySegment<byte>(Encoding.UTF8.GetBytes("ping"));
                                await webSocket.SendAsync(pingMessage, WebSocketMessageType.Text, true, CancellationToken.None);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ping error: {ex.Message}");
                                break;  
                            }
                        }
                    }
                });

                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    var changeIdPref = sendBy == "client" ? clientId + "A" : clientId + "B";

                    if (result.CloseStatus.HasValue)
                    {
                        _clients.TryRemove(changeIdPref, out _);
                        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                        break;
                    }

                    var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var splitMessage = message.Split(':', 2);

                    if (splitMessage.Length == 2)
                    {
                        var recipientId = splitMessage[0].Trim();
                        var msgContent = splitMessage[1].Trim();
                        ChatModel md = new ChatModel();
                        md.senderId = Convert.ToInt32(clientId);
                        md.message = msgContent;
                        md.receiverId = Convert.ToInt32(recipientId);
                        md.mssDate = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");
                        md.sendBy = sendBy;
                        md.status = 1;

                        ChatedUserViewModel cu = new ChatedUserViewModel();
                        cu.Status = 1;
                        cu.UserId = sendBy == "client" ? Convert.ToInt32(clientId):Convert.ToInt32(recipientId);
                        cu.JyotishId = sendBy == "client" ?Convert.ToInt32(recipientId):Convert.ToInt32(clientId);
                        cu.FirstMessageAt = DateTime.Now;
                        cu.LastMessageAt = DateTime.Now;


                        var cres = _chat.AddChatUser(cu);
                        var res = _chat.AddChat(md);
                        var changeresPref = sendBy == "client" ? recipientId + "B" : recipientId + "A";

        
                        if (_clients.TryGetValue(changeresPref, out var recipientSocket))
                        {
                            ReceiveChat ms = new ReceiveChat
                            {
                                mssg = msgContent,
                                date = DateTime.Now.ToString("hh:mm tt")
                            };
                            
                            string jsonString = JsonConvert.SerializeObject(ms);
                            var msgBuffer = System.Text.Encoding.UTF8.GetBytes(jsonString);
                            await recipientSocket.SendAsync(new ArraySegment<byte>(msgBuffer), result.MessageType, result.EndOfMessage, CancellationToken.None);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
           
        }
       

        [HttpGet("getchats")]
        public List<ChatModel> GetChats(int sender, int receiver)
        {
            var result = _chat.GetChats(sender, receiver);
            return result;
        }
        [HttpGet("getChatedUser")]
        public dynamic GetChatedUser(int id, string userType)
        {
            var result = _chat.GetChatedUser(id, userType);
            return result;
        }
    }
}
