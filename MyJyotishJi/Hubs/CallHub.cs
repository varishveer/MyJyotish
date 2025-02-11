
using BusinessAccessLayer.Abstraction;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace BusinessAccessLayer.Implementation
{
    public class CallHub : Hub
    {// Tracks pending call requests using a key "senderId-targetId".
        private readonly IUserServices _service;
        private readonly IJyotishServices _jyotish;
        private static readonly HashSet<string> pendingCallRequests = new();
        private static readonly Dictionary<string, string> connectedClients = new(); // Store the connection IDs for each user
        private static Dictionary<string, DateTime> _callTimeManager = new();
        private static readonly Dictionary<string, string> receiverSenderConnectionId = new();
        public CallHub(IUserServices service, IJyotishServices jyotish)
        {
            _service = service;
            _jyotish = jyotish;
        }
        /// <summary>
        /// Handles client connection to the hub.
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            // Optionally, you can associate the connection ID with a user ID (if using authentication).
            string userId = Context.GetHttpContext()?.Request.Query["userId"];  // Assuming userId is passed in the query string
            string sendby = Context.GetHttpContext()?.Request.Query["sendby"];  // Assuming userId is passed in the query string
            var senderId = sendby == "client" ? userId + "_client" : userId + "_jyotish";

            try
            {

                // This will be triggered when a client successfully connects.
                string connectionId = Context.ConnectionId;
                Console.WriteLine($"Client connected: {connectionId}");

                if (!string.IsNullOrEmpty(userId))
                {
                    connectedClients[senderId] = connectionId;
                    Console.WriteLine($"User {senderId} connected with connection ID {connectionId}");
                    if (sendby == "client")
                    {
                        Task.Run(() =>
                        {
                            _service.changeUserServiceStatus(int.Parse(userId), true);
                        }).Wait();
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            _jyotish.changeJyotishServiceStatus(int.Parse(userId), true);

                        }).Wait();


                    }
                }

                // Proceed with the rest of the connection logic
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    if (sendby == "client")
                    {
                       
                            _service.changeUserServiceStatus(int.Parse(userId), false);
                        
                    }
                    else
                    {
                        
                            _jyotish.changeJyotishServiceStatus(int.Parse(userId), false);
                       
                    }
                }
            }
        }

        public async Task manageUserPayment()
        {
            var userId = connectedClients.FirstOrDefault(e => e.Value == Context.ConnectionId).Key;

            if (!string.IsNullOrEmpty(userId))
            {

                    if (!_callTimeManager.ContainsKey(userId))
                    {
                        _callTimeManager.Add(userId, DateTime.Now);
                    }
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ClientDisconnet");

            }
        }

        /// <summary>
        /// Handles client disconnection from the hub.
        /// </summary>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;
            string userId = connectedClients.FirstOrDefault(c => c.Value == connectionId).Key;
            try
            {
                // This will be triggered when a client disconnects.
                Console.WriteLine($"Client disconnected: {connectionId}");
                // Optionally, you can find the associated user ID and clean up.

                if (!string.IsNullOrEmpty(userId))
                {
                    var senderId = userId.Split("_")[0];
                    var senderType = userId.Split("_")[1];
                    if (senderType == "client")
                    {
                        if (receiverSenderConnectionId.ContainsKey(Context.ConnectionId))
                        {
                            var receiverId = receiverSenderConnectionId.FirstOrDefault(e => e.Key == Context.ConnectionId).Value;
                            var jyotishData = connectedClients.FirstOrDefault(e => e.Value == receiverId).Key;

                            if (!string.IsNullOrEmpty(jyotishData) && _callTimeManager.ContainsKey(userId))
                            {
                                var id = jyotishData.Split("_")[0];
                                int getJyotishchatCharges = 0;
                                Task.Run(() =>
                                {
                                    getJyotishchatCharges = _service.getJyotishCallServicesCharges(int.Parse(id));
                                }).Wait();
                                if (_callTimeManager.ContainsKey(userId))
                                {
                                    var dateDifference = _callTimeManager.Where(e => e.Key == userId).First().Value - DateTime.Now;
                                    var totalMinutes = Math.Ceiling(Math.Abs(dateDifference.TotalMinutes));
                                    var totalAmount = getJyotishchatCharges * totalMinutes;
                                    _callTimeManager.Remove(userId);
                                    string messages = "call with astrologers";
                                    Task.Run(() =>
                                    {
                                        _service.ApplyChargesFromUserWalletForService(int.Parse(senderId), Convert.ToInt32(totalAmount), messages, int.Parse(id));
                                    }).Wait();
                                }
                                receiverSenderConnectionId.Remove(Context.ConnectionId);
                                await Clients.Client(receiverId).SendAsync("ClientDisconnet");
                            }
                        }
                        
                            _service.changeUserServiceStatus(int.Parse(senderId), false);
                       
                    }
                    else
                    {
                        if (receiverSenderConnectionId.ContainsValue(Context.ConnectionId))
                        {

                            var receiverId = receiverSenderConnectionId.FirstOrDefault(e => e.Value == Context.ConnectionId).Key;
                            var userData = connectedClients.FirstOrDefault(e => e.Value == receiverId).Key;

                            if (!string.IsNullOrEmpty(userData) && _callTimeManager.ContainsKey(userData))
                            {
                                var id = userData.Split("_")[0];
                                int getJyotishchatCharges = 0;
                                Task.Run(() =>
                                {
                                    getJyotishchatCharges = _service.getJyotishCallServicesCharges(int.Parse(senderId));
                                }).Wait();
                                var dateDifference = _callTimeManager.Where(e => e.Key == userData).First().Value - DateTime.Now;
                                var totalMinutes = Math.Ceiling(Math.Abs(dateDifference.TotalMinutes));
                                var totalAmount = getJyotishchatCharges * totalMinutes;
                                _callTimeManager.Remove(userData);
                                string messages = "call with astrologers";
                                Task.Run(() =>
                                {
                                    _service.ApplyChargesFromUserWalletForService(int.Parse(id), Convert.ToInt32(totalAmount), messages, int.Parse(senderId));
                                }).Wait();
                            }

                            receiverSenderConnectionId.Remove(receiverId);
                            await Clients.Client(receiverId).SendAsync("ClientDisconnet");

                        }
                     
                            _jyotish.changeJyotishServiceStatus(int.Parse(senderId), false);
                       
                    }
                    connectedClients.Remove(userId);
                    Console.WriteLine($"User {userId} disconnected, removed connection ID {connectionId}");
                }

                var toRemoveNull = pendingCallRequests.Where(r => r==null).ToList();
                foreach (var request in toRemoveNull)
                {
                    pendingCallRequests.Remove(request);

                    Console.WriteLine($"Removed pending call request: {request}");
                }
                // Clean up pending call requests related to this user
                var toRemove = pendingCallRequests.Where(r => r.StartsWith(userId + "-") || r.EndsWith("-" + userId)).ToList();
                foreach (var request in toRemove)
                {
                    pendingCallRequests.Remove(request);

                    Console.WriteLine($"Removed pending call request: {request}");
                }
                // Proceed with the rest of the disconnection logic
                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var senderId = userId.Split("_")[0];
                    var senderType = userId.Split("_")[1];
                    if (senderType == "client")
                    {
                        
                            _service.changeUserServiceStatus(int.Parse(senderId), false);
                        
                    }
                    else
                    {
                       
                            _jyotish.changeJyotishServiceStatus(int.Parse(senderId), false);
                        

                    }
                }
            }
        }

        /// <summary>
        /// Receives signaling data from a client and forwards it to the target client.
        /// If the signal is a call request, check that it has not been sent already.
        /// </summary>
        public async Task SendSignal(string senderConnectionId, string targetConnectionId, string signalData, string sendby)
        {
            // Try to parse the incoming signal as JSON.
            dynamic signalObj = null;
            try
            {
                signalObj = JsonConvert.DeserializeObject(signalData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid signal data: " + ex.Message);
                throw;
            }

            string type = signalObj?.type;
            long totaltimeforcall = 0;
            if (type == "callRequest")
            {
                // Create a unique key for the request.
                string requestKey = $"{senderConnectionId}-{targetConnectionId}";

                // If a request with this key is already pending, do nothing.
                if (pendingCallRequests.Contains(requestKey))
                {
                    Console.WriteLine($"Call request from {senderConnectionId} to {targetConnectionId} is already pending.");
                    return;
                }
                else
                {
                    pendingCallRequests.Add(requestKey);
                        string clientId = connectedClients.FirstOrDefault(c => c.Value == senderConnectionId).Key;
                        var userData = connectedClients.FirstOrDefault(e => e.Value == targetConnectionId).Key;
                            if (string.IsNullOrEmpty(userData)) return;
                    if (sendby == "client")
                    {
                        if (!receiverSenderConnectionId.ContainsKey(senderConnectionId)) receiverSenderConnectionId[senderConnectionId] = targetConnectionId;
                       
                    }
                    else
                    {
                        var id = clientId.Split("_")[0];
                        var uId = userData.Split("_")[0];
                        Task.Run(() =>
                        {
                            var totalWalletAmount = _service.GetWallet(int.Parse(uId));
                            var getJyotishchatCharges = _service.getJyotishCallServicesCharges(int.Parse(id));
                            totaltimeforcall = getJyotishchatCharges > 0 ? totalWalletAmount / getJyotishchatCharges : 0;
                        }).Wait();
                    }
                }
            }

            // Forward the signal to the target client.
            await Clients.Client(targetConnectionId).SendAsync("ReceiveSignal", senderConnectionId, signalData, totaltimeforcall);

        }

        /// <summary>
        /// Optional: Remove the pending call request once it is handled (accepted or rejected).
        /// This can be called from another hub method or via a dedicated client call.
        /// </summary>
        public static void RemovePendingCallRequest(string senderConnectionId, string targetConnectionId)
        {
            string requestKey = $"{senderConnectionId}-{targetConnectionId}";
            if (pendingCallRequests.Contains(requestKey))
            {
                pendingCallRequests.Remove(requestKey);
            }
        }
    }
}


