using Azure.Core;
using BusinessAccessLayer.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelAccessLayer.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Security.Policy;
using System.Text;
using Twilio;
using Twilio.Jwt.AccessToken;
using Twilio.TwiML;
using Twilio.Types;

namespace MyJyotishGApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallController : ControllerBase
    {
        private static readonly ConcurrentDictionary<string, WebSocket> _clientRequest = new();
        private static Dictionary<string, string> _clientRequestMessage = new();
        private static Dictionary<string, string> _clientRoomId= new Dictionary<string, string>();
        private readonly IUserServices _services;
        public CallController( IUserServices services)
        {
            _services = services;
        }

        [HttpGet("sendCallRequest")]
        public async Task SendChatRequest(string id, string sendBy)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var changeIdPref = sendBy == "client" ? id + "A" : id + "B";
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _clientRequest[changeIdPref] = webSocket;

                if (_clientRequest.TryGetValue(changeIdPref, out var recipientSocket))
                {
                    dynamic userRequestRecord = null;
                    if (sendBy != "client")
                    {
                        userRequestRecord = _clientRequestMessage.ContainsKey(id) ? _clientRequestMessage.Where(e => e.Key == id).First().Value : null;
                        dynamic roomId = null;
                        if (_clientRoomId != null)
                        {

                         roomId = _clientRoomId.ContainsKey(id)?_clientRoomId.Where(e => e.Key == id).First().Value:null;
                        }
                        string jsonString = JsonConvert.SerializeObject(new { status = true, type = "call",roomId=roomId , data = userRequestRecord });
                        var msgBuffer = System.Text.Encoding.UTF8.GetBytes(jsonString);
                        await recipientSocket.SendAsync(new ArraySegment<byte>(msgBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
                await HandleCallRequest(webSocket, id, sendBy);
            }
            else
            {
                HttpContext.Response.StatusCode = 400; // Bad Request
            }
        }

        private async Task HandleCallRequest(WebSocket webSocket, string clientId, string sendBy)
        {
            var buffer = new byte[1024 * 4];
            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    var changeIdPref = sendBy == "client" ? clientId + "A" : clientId + "B";
                    if (result.CloseStatus.HasValue)
                    {
                        if (sendBy == "client")
                        {
                            var castId = Convert.ToInt32(clientId);
                            var userDetail = _services.LayoutData(castId);
                            string jsonString = JsonConvert.SerializeObject(userDetail);

                            var clientKey = _clientRequestMessage.FirstOrDefault(e => e.Value.Equals(jsonString)).Key;
                            if (clientKey != null)
                            {
                                var changeresPref = clientKey + "B";
                                if (_clientRequest.TryGetValue(changeresPref, out var recipientSocket))
                                {
                                    _clientRequestMessage.Remove(clientKey);
                                    _clientRoomId.Remove(clientKey);
                                    string jsonStrings = JsonConvert.SerializeObject(new { status = true, type = "call", data = false });
                                    var msgBuffer = System.Text.Encoding.UTF8.GetBytes(jsonStrings);
                                    await recipientSocket.SendAsync(new ArraySegment<byte>(msgBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                                }
                            }
                        }

                        _clientRequest.TryRemove(changeIdPref, out _);

                        await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Closing connection", CancellationToken.None);

                        break;
                    }

                    var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);

                    if (message != null)
                    {
                    var splitMessage = message.Split(':', 2);
                        var recipientId = splitMessage[0].Trim();
                        var roomId = splitMessage[1].Trim();
                        var changeresPref = sendBy == "client" ? recipientId + "B" : recipientId + "A";
                        dynamic userRequestRecord = new {room=false };
                        if (sendBy == "client")
                        {
                            var castId = Convert.ToInt32(clientId);
                            var userDetail = _services.LayoutData(castId);
                            string userJson = JsonConvert.SerializeObject(userDetail);
                            if (!_clientRequestMessage.ContainsKey(recipientId) && !string.IsNullOrEmpty(recipientId) && _clientRequestMessage.Count==0)
                            {
                                _clientRequestMessage.Add(recipientId, userJson);
                                if (!_clientRoomId.ContainsKey(recipientId) && !string.IsNullOrEmpty(recipientId))
                                {
                                    _clientRoomId.Add(recipientId, roomId);
                                }
                            };

                            
                        }
                        if (_clientRequest.TryGetValue(changeresPref, out var recipientSocket))
                        {
                            if (sendBy != "client")
                            {
                                userRequestRecord = new { room=true};
                            }
                            else
                            {
                               
                                userRequestRecord = _clientRequestMessage.ContainsKey(recipientId) ? _clientRequestMessage.Where(e => e.Key == recipientId).First().Value : null;
                            }

                            string jsonString = JsonConvert.SerializeObject(new { status = true, type = "call",roomId=roomId, data = userRequestRecord });
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


    }

}
