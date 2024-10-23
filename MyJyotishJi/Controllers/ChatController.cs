﻿using BusinessAccessLayer.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelAccessLayer.Models;
using System.Collections.Concurrent;
using System.Net.WebSockets;

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
               
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _clients[id] = webSocket;

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
            try
            {
                while (true)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.CloseStatus.HasValue)
                    {
                        _clients.TryRemove(clientId, out _);
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
                        md.mssDate = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                        md.sendBy = sendBy;
                        md.status = 1;

                        ChatedUser cu = new ChatedUser();
                        cu.Status = 1;
                        cu.UserId = sendBy == "client" ? Convert.ToInt32(clientId):Convert.ToInt32(recipientId);
                        cu.JyotishId = sendBy == "client" ?Convert.ToInt32(recipientId):Convert.ToInt32(clientId);
                        cu.FirstMessageAt = DateTime.Now;
                        cu.LastMessageAt = DateTime.Now;


                        var cres = _chat.AddChatUser(cu);
                        var res = _chat.AddChat(md);

                        if (_clients.TryGetValue(recipientId, out var recipientSocket))
                        {
                            var msgBuffer = System.Text.Encoding.UTF8.GetBytes($"{clientId}: {msgContent}");
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
        public List<ChatedUser> GetChatedUser(int id, string userType)
        {
            var result = _chat.GetChatedUser(id, userType);
            return result;
        }
    }
}
