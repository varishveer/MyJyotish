
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace BusinessAccessLayer.Implementation
{
    public class CallHub : Hub
    {
        // Method to handle sending an ICE candidate
        public async Task SendIceCandidate(string toUserId, string candidate)
        {
            if (string.IsNullOrEmpty(toUserId))
            {
                throw new ArgumentException("Recipient user ID is required.", nameof(toUserId));
            }

            if (string.IsNullOrEmpty(candidate))
            {
                throw new ArgumentException("ICE candidate is required.", nameof(candidate));
            }

            try
            {
                // Send the ICE candidate to the intended recipient
                await Clients.User(toUserId).SendAsync("ReceiveIceCandidate", candidate);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error sending ICE candidate: {ex.Message}");
                throw;  // Rethrow the error to let the client know about the failure
            }
        }

        // Method to handle sending a call offer
        public async Task SendCallOffer(string toUserId, string offer)
        {
            if (string.IsNullOrEmpty(toUserId) || string.IsNullOrEmpty(offer))
            {
                throw new ArgumentException("Invalid parameters received in SendCallOffer");
            }

            try
            {
                // Send the offer to the target user
                await Clients.User(toUserId).SendAsync("ReceiveCallOffer", offer);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error sending call offer: {ex.Message}");
                throw;  // Rethrow the error to let the client know about the failure
            }
        }

        // Method to handle the call answer
        public async Task SendCallAnswer(string toUserId, string answer)
        {
            if (string.IsNullOrEmpty(toUserId))
            {
                throw new ArgumentException("Recipient user ID is required.", nameof(toUserId));
            }

            if (string.IsNullOrEmpty(answer))
            {
                throw new ArgumentException("Answer is required.", nameof(answer));
            }

            try
            {
                // Send the answer to the recipient user
                await Clients.User(toUserId).SendAsync("ReceiveCallAnswer", answer);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error sending call answer: {ex.Message}");
                throw;  // Rethrow the error to let the client know about the failure
            }
        }

        // Method to handle hang-up calls
        public async Task SendHangUp(string toUserId)
        {
            if (string.IsNullOrEmpty(toUserId))
            {
                throw new ArgumentException("Recipient user ID is required.", nameof(toUserId));
            }

            try
            {
                // Notify the recipient user that the call has ended
                await Clients.User(toUserId).SendAsync("ReceiveHangUp");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error sending hang up: {ex.Message}");
                throw;  // Rethrow the error to let the client know about the failure
            }
        }
    }

}
