using BusinessAccessLayer.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Text;

namespace MyJyotishGApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private static readonly ConcurrentDictionary<string, WebSocket> _clients = new();
        private static Dictionary<string, DateTime> _chatTimeManager = new();
        private static Dictionary<string, long> _userWalletAmount = new();

        private readonly IChat _chat;
        private readonly IUserServices _services;
        private readonly IJyotishServices _jyotish;

        public ChatController(IChat chat, IUserServices services, IJyotishServices jyotish)
        {
            _chat = chat;
            _services = services;
            _jyotish = jyotish;
        }

        [HttpGet("connect")]
        public async Task Connect(string id, string receiverId, string sendBy)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                try
                {
                    var changeIdPref = sendBy == "client" ? id + "A" : id + "B";
                    var changeIdPrefReceiver = sendBy == "client" ? receiverId + "B" : receiverId + "A";
                    var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                    _clients[changeIdPref] = webSocket;

                    if (sendBy == "client")
                    {

                        if (_clients.TryGetValue(changeIdPref, out var recipientSocket))
                        {
                            var totalWalletAmount = _services.GetWallet(int.Parse(id));
                            var getJyotishchatCharges = _services.getJyotishServicesCharges(int.Parse(receiverId));
                            var totaltimeforchat = getJyotishchatCharges > 0
                           ? (long)(totalWalletAmount / getJyotishchatCharges)
                           : 0;
                            if (_userWalletAmount.ContainsKey(id))
                            {
                                _userWalletAmount.Remove(id);
                            }
                            _userWalletAmount.Add(id, totalWalletAmount);
                            string jsonString = JsonConvert.SerializeObject(new { status = true, type = "chatPayment", connection = true, totalTime = totaltimeforchat, totalAmount = totalWalletAmount });
                            
                            
                            var msgBuffer = System.Text.Encoding.UTF8.GetBytes(jsonString);
                            await recipientSocket.SendAsync(new ArraySegment<byte>(msgBuffer), WebSocketMessageType.Text, true, CancellationToken.None);

                        }
                            _services.changeUserServiceStatus(int.Parse(id), true);
                        if (!_chatTimeManager.ContainsKey(id))
                        {

                            _chatTimeManager.Add(id, DateTime.Now);
                        }
                    }
                   
                   
                    if (_clients.TryGetValue(changeIdPrefReceiver, out var recipientSocketForConnect))
                        {
                        ReceiveChat ms = new ReceiveChat
                        {
                            mssg = sendBy == "client" ? "Client are connected" : "Jyotish are connected",
                            date = DateTime.Now.ToString("hh:mm tt")
                        };
                            var msgBuffer = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ms));
                            await recipientSocketForConnect.SendAsync(new ArraySegment<byte>(msgBuffer), WebSocketMessageType.Text, true, CancellationToken.None);

                        }
                    await HandleWebSocketCommunication(webSocket, id, receiverId, sendBy);
                }catch(Exception ex)
                {
                    throw ex;
                }

            }
            else
            {
                HttpContext.Response.StatusCode = 400; // Bad Request
            }
        }

        private async Task HandleWebSocketCommunication(WebSocket webSocket, string clientId, string receiverId, string sendBy)
        {
            var buffer = new byte[1024 * 4];

            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.CloseStatus.HasValue)
                    {

                    var changeIdPref = sendBy == "client" ? receiverId + "B" : receiverId + "A";
                    var RemoveRequest = sendBy == "client" ? clientId + "A" : clientId + "B";
                       
                        var userId = sendBy == "client" ? clientId : receiverId;
                        if (sendBy == "client")
                        {
                            _services.changeUserServiceStatus(int.Parse(clientId), false);

                        }
                        if (_userWalletAmount.ContainsKey(userId))
                        {

                            var jyotishId = sendBy == "client" ? receiverId : clientId;
                            var getJyotishchatCharges = _services.getJyotishServicesCharges(int.Parse(jyotishId));
                            var dateDifference = _chatTimeManager.Where(e => e.Key == userId).First().Value - DateTime.Now;
                            var totalMinutes = Math.Ceiling(Math.Abs(dateDifference.TotalMinutes));
                            var totalAmount = getJyotishchatCharges * totalMinutes; 
                            _userWalletAmount.Remove(userId);
                            
                            string messages = "Chat with astrologers";
                            var res = _services.ApplyChargesFromUserWalletForService(int.Parse(userId),Convert.ToInt32(totalAmount), messages, int.Parse(jyotishId));
                        }
                        ReceiveChat ms = new ReceiveChat
                        {
                            mssg = sendBy == "client" ? "Client Are Disconnected" : "Jyotish Are Disconnected",
                            date = DateTime.Now.ToString("hh:mm tt")
                        };
                        if (_clients.TryGetValue(changeIdPref, out var recipientSocket))
                        {
                            string jsonString = JsonConvert.SerializeObject(ms);
                            var msgBuffer = System.Text.Encoding.UTF8.GetBytes(jsonString);
                            await recipientSocket.SendAsync(new ArraySegment<byte>(msgBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                        _clients.TryRemove(RemoveRequest, out _);
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
                        cu.UserId = sendBy == "client" ? Convert.ToInt32(clientId) : Convert.ToInt32(recipientId);
                        cu.JyotishId = sendBy == "client" ? Convert.ToInt32(recipientId) : Convert.ToInt32(clientId);
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

        private static readonly ConcurrentDictionary<string, WebSocket> _clientRequest = new();
        private static Dictionary<string, string> _clientRequestMessage = new();
        private static Dictionary<string, string> _clientRoomId = new();
        private static Dictionary<string, DateTime> _RequestManager = new();

        [HttpGet("sendChatRequest")]
        public async Task SendChatRequest(string id,string receiverId, string sendBy)
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
                        _jyotish.changeJyotishActiveStatus(int.Parse(id), true);

                        userRequestRecord = _clientRequestMessage.ContainsKey(id) ? _clientRequestMessage.Where(e => e.Key == id).First().Value : null;

                        string roomId = null;
                        if (_clientRoomId != null)
                        {
                            roomId = _clientRoomId.ContainsKey(id) ? _clientRoomId.Where(e => e.Key == id).First().Value : null;
                        }
                        string jsonString = JsonConvert.SerializeObject(new { status = true, type = roomId == null ? "chat" : "call", roomId = roomId, data = userRequestRecord });
                        var msgBuffer = System.Text.Encoding.UTF8.GetBytes(jsonString);
                        await recipientSocket.SendAsync(new ArraySegment<byte>(msgBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                        await HandleChatRequest(webSocket, id, receiverId, sendBy);

                    }
                    else
                    {
                            var checkServiceStatus = _services.getUserserviceStatus(int.Parse(id));
                            if (!checkServiceStatus)
                            {
                            _services.changeUserServiceStatus(int.Parse(id),true);
                await HandleChatRequest(webSocket, id, receiverId, sendBy);
                            }
                            else
                            {
                            _clientRequest.TryRemove(changeIdPref, out _);
                            string jsonString = JsonConvert.SerializeObject(new { status = true, data = false, anotherRequest = true });
                              var msgBuffer = System.Text.Encoding.UTF8.GetBytes(jsonString);
                              await recipientSocket.SendAsync(new ArraySegment<byte>(msgBuffer), WebSocketMessageType.Text, true, CancellationToken.None);

                        }

                    }
                }
            }
            else
            {
                HttpContext.Response.StatusCode = 400; // Bad Request
            }
        }

        private async Task HandleChatRequest(WebSocket webSocket, string clientId,string receiverId, string sendBy)
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
                           

                            var clientKey = receiverId;
                            if (clientKey != null)
                            {
                                var changeresPref = receiverId + "B";

                                if (_clientRequest.TryGetValue(changeresPref, out var recipientSocket))
                                {
                                    _clientRequestMessage.Remove(receiverId);
                                    string jsonStrings = JsonConvert.SerializeObject(new { status = true, type = "chat", data = false });
                                    if (_clientRoomId.ContainsKey(receiverId))
                                    {
                                        _clientRoomId.Remove(receiverId);
                                        jsonStrings = JsonConvert.SerializeObject(new { status = true, type = "call", roomId = false, data = false });
                                    }

                                    var msgBuffer = System.Text.Encoding.UTF8.GetBytes(jsonStrings);

                                    await recipientSocket.SendAsync(new ArraySegment<byte>(msgBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                                }
                            }
                            if (sendBy == "client")
                            {
                                _services.changeUserServiceStatus(castId,false);
                            }
                        }
                        else
                        {
                            _jyotish.changeJyotishActiveStatus(int.Parse(clientId), false);

                        }
                        _clientRequest.TryRemove(changeIdPref, out _);

                        await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Closing connection", CancellationToken.None);

                        break;
                    }
                    var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                    if (message != null)
                    {
                        var splitMessage = message.Split(':', 3);
                        var recipientId = splitMessage[0].Trim();
                        var roomId = splitMessage[1].Trim();
                        var requestType = splitMessage[2].Trim();
                        var changeresPref = sendBy == "client" ? recipientId + "B" : recipientId + "A";
                        dynamic userRequestRecord = false;
                        if (sendBy == "client")
                        {
                            var castId = Convert.ToInt32(clientId);
                            var userDetail = _services.LayoutData(castId);
                            string userJson = JsonConvert.SerializeObject(userDetail);

                            if (_RequestManager.Count > 0)
                            {
                                if (_RequestManager.ContainsKey(recipientId))
                                {
                                    var dateDifference = _RequestManager.Where(e => e.Key == recipientId).First().Value - DateTime.Now;
                                    if (dateDifference.Duration() >= TimeSpan.FromSeconds(120))
                                    {
                                        string userJsonDetail = null;
                                        if (_clientRequestMessage.Count > 0)
                                        {
                                            userJsonDetail = _clientRequestMessage.ContainsKey(recipientId) ? _clientRequestMessage.Where(e => e.Key == recipientId).First().Value : null;
                                            _clientRequestMessage.Remove(recipientId);
                                        }
                                        if (_clientRoomId.Count > 0)
                                        {
                                            _clientRoomId.Remove(recipientId);
                                        }
                                        _RequestManager.Remove(recipientId);
                                        if (!string.IsNullOrEmpty(userJsonDetail) && !userJsonDetail.Equals("Null") && !userJsonDetail.Equals("null"))
                                        {
                                            JObject jsonObject = JObject.Parse(userJsonDetail);
                                            var clientChangeref = jsonObject["Id"] + "A";
                                            if (_clientRequest.TryGetValue(clientChangeref, out var ClientSocket))
                                            {
                                                string jsonStrings = JsonConvert.SerializeObject(new { status = true, data = false, anotherRequest = false });
                                                if (clientId == jsonObject["Id"].ToString())
                                                {
                                                    jsonStrings = JsonConvert.SerializeObject(new { status = true, data = false, anotherRequest = true });
                                                }
                                                var msgBuffer = System.Text.Encoding.UTF8.GetBytes(jsonStrings);

                                                await ClientSocket.SendAsync(new ArraySegment<byte>(msgBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                                            }
                                        }

                                    }
                                }
                            }

                            if (!_clientRequestMessage.ContainsKey(recipientId) && !string.IsNullOrEmpty(recipientId))
                            {
                                _clientRequestMessage.Add(recipientId, userJson);
                                if (!_clientRoomId.ContainsKey(recipientId) && !string.IsNullOrEmpty(recipientId) && requestType == "call")
                                {
                                    _clientRoomId.Add(recipientId, roomId);
                                }
                                if (!_RequestManager.ContainsKey(recipientId))
                                {

                                _RequestManager.Add(recipientId, DateTime.Now);
                                }
                            }

                        }
                        else
                        {
                            if (_clientRequestMessage.Count > 0)
                            {
                                if (_clientRequestMessage.ContainsKey(clientId))
                                {
                                    _clientRequestMessage.Remove(clientId);
                                }
                                if (_clientRoomId.ContainsKey(clientId))
                                {
                                    _clientRoomId.Remove(clientId);
                                }
                                if (_RequestManager.ContainsKey(clientId))
                                {
                                    _RequestManager.Remove(clientId);
                                }
                                var changeresPrefjy = clientId + "B";

                                if (_clientRequest.TryGetValue(changeresPrefjy, out var recipientSocketjy))
                                {
                                    var jsonStrings = JsonConvert.SerializeObject(new { status = true, type = "call", roomId = false, data = false });
                                    var msgBuffer = System.Text.Encoding.UTF8.GetBytes(jsonStrings);

                                    await recipientSocketjy.SendAsync(new ArraySegment<byte>(msgBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                                }
                            }
                        }
                        if (_clientRequest.TryGetValue(changeresPref, out var recipientSocket))
                        {
                            if (sendBy != "client")
                            {
                                userRequestRecord = true;
                            }
                            else
                            {
                                userRequestRecord = userRequestRecord = _clientRequestMessage.ContainsKey(recipientId) ? _clientRequestMessage.Where(e => e.Key == recipientId).First().Value : null;
                            }
                            string jsonString = JsonConvert.SerializeObject(new { status = true, type = requestType == "call" && sendBy == "client" ? _clientRoomId.Count > 0 ? "call" : "chat" : requestType, roomId = requestType == "call" && sendBy == "client" && _clientRoomId.Count > 0 ? _clientRoomId.Where(e => e.Key == recipientId).First().Value : roomId = requestType == "call" && sendBy != "client" && _clientRoomId.Count > 0 ? _clientRoomId.Where(e => e.Key == clientId).First().Value : null, data = userRequestRecord });
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
