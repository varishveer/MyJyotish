using Azure.Core;
using BusinessAccessLayer.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelAccessLayer.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
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
        private static readonly ConcurrentDictionary<string, WebSocket> _clientCallRequest = new();
        private static Dictionary<string, string> _clientCallRequestMessage = new();

        private static readonly ConcurrentDictionary<string, WebSocket> _clientRequest = new();
        private readonly IUserServices _services;
        public CallController( IUserServices services)
        {
            _services = services;
        }
        [HttpGet("sendcallRequest")]
        public async Task SendCallRequest(string id, string sendBy)
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
                        userRequestRecord = _clientCallRequestMessage.ContainsKey(id) ? _clientCallRequestMessage.Where(e => e.Key == id).First().Value : null;

                        string jsonString = JsonConvert.SerializeObject(new { status = true, type = "call", data = userRequestRecord });
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

                            var clientKey = _clientCallRequestMessage.FirstOrDefault(e => e.Value.Equals(jsonString)).Key;
                            if (clientKey != null)
                            {
                                var changeresPref = clientKey + "B";

                                if (_clientCallRequest.TryGetValue(changeresPref, out var recipientSocket))
                                {
                                    _clientCallRequestMessage.Remove(clientKey);

                                    string jsonStrings = JsonConvert.SerializeObject(new { status = true, type = "chat", data = false });
                                    var msgBuffer = System.Text.Encoding.UTF8.GetBytes(jsonStrings);

                                    await recipientSocket.SendAsync(new ArraySegment<byte>(msgBuffer), WebSocketMessageType.Text, true, CancellationToken.None);

                                }
                            }
                        }

                        _clientCallRequest.TryRemove(changeIdPref, out _);

                        await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Closing connection", CancellationToken.None);

                        break;
                    }


                    var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);

                    if (message != null)
                    {
                        var recipientId = message.Trim();
                        var changeresPref = sendBy == "client" ? recipientId + "B" : recipientId + "A";
                        dynamic userRequestRecord = false;
                        if (sendBy == "client")
                        {
                            var castId = Convert.ToInt32(clientId);
                            var userDetail = _services.LayoutData(castId);
                            string userJson = JsonConvert.SerializeObject(userDetail);
                            if (!_clientCallRequestMessage.ContainsKey(recipientId) && !string.IsNullOrEmpty(recipientId) && _clientCallRequestMessage.Count==0)
                            {
                                _clientCallRequestMessage.Add(recipientId, userJson);
                            }
                            ;
                        }
                        if (_clientCallRequest.TryGetValue(changeresPref, out var recipientSocket))
                        {
                            if (sendBy != "client")
                            {
                                userRequestRecord = true;
                            }
                            else
                            {

                                userRequestRecord = userRequestRecord = _clientCallRequestMessage.ContainsKey(recipientId) ? _clientCallRequestMessage.Where(e => e.Key == recipientId).First().Value : null;
                            }

                            string jsonString = JsonConvert.SerializeObject(new { status = true, type = "chat", data = userRequestRecord });
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
