using BusinessAccessLayer.Abstraction;
using DataAccessLayer.Migrations;
using Microsoft.AspNetCore.SignalR;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using ModelAccessLayer.ViewModels;
using NuGet.Protocol.Plugins;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyJyotishGApi.Hubs
{
    public class SendChatRequest:Hub
    {

        private static readonly Dictionary<string, string> _connections = new Dictionary<string, string>();
        private readonly IUserServices _services;
        private string userconenctorId;
        private static readonly Dictionary<string,dynamic> _message = new Dictionary<string, dynamic>();
        // Connect a user to the hub and store their connection ID with their user ID
        public async Task ConnectUser(string userId,string sendBy)
        {
           var newUserId = sendBy == "client" ? userId + "A" : userId + "B";
            userconenctorId = newUserId;
            _connections[newUserId] = Context.ConnectionId;
            await SendMessageToJyotish(userId);
            if (sendBy != "client")
            {
                if (_message.TryGetValue(userconenctorId, out var message))
                {
                    await Clients.Client(_connections[userconenctorId]).SendAsync("ReceiveMessage", (LayoutDataViewModel)message);
                }
            }
            await Clients.Caller.SendAsync("Connected", userId); 
        }

        // Send a message to a specific user
        public async Task SendMessageToUser(string userId)
        {
            var newUserId = userId + "A" ;

            if (_connections.ContainsKey(newUserId))
            {

                var connectionId = _connections[newUserId];
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", true);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "User not connected.");
            }
        }

        // Send a message to the admin
        public async Task SendMessageToJyotish(string userId)
        {
            if (userId != null)
            {
                var newUserId = userId + "B";
                var castUserId = Convert.ToInt32(userId);
                var userDetail = _services.LayoutData(castUserId);
                _message.Add(newUserId, userDetail);
                // Admin is connected
                await Clients.Client(_connections[newUserId]).SendAsync("ReceiveMessage", userDetail);
            }
            else
            {
                
                await Clients.Caller.SendAsync("Error", "Admin is not connected.");
            }
        }

        // Method to check if a user is connected (find connection by user ID)
        public bool IsUserConnected(string userId)
        {
            return _connections.ContainsKey(userId);
        }

        // Method to disconnect admin
        public async Task DisconnectJyotish()
        {
            userconenctorId = null;
            await Clients.Caller.SendAsync("JyotishDisconnected", "Admin has disconnected.");
        }

        // Disconnect user and remove their connection from the dictionary
        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            // Remove the disconnected user from the dictionary
            var userId = _connections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (userId != null)
            {
                _connections.Remove(userId);
            }

            // If admin disconnects, reset their connection ID
            if (Context.ConnectionId == userconenctorId)
            {
                userconenctorId = null;
            }

            await base.OnDisconnectedAsync(exception);
        }

    }
}
