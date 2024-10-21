using BusinessAccessLayer.Abstraction;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessAccessLayer.Implementation
{
    public class CallServices:ICallServices
    {
       /* private readonly IHubContext<CallHub> _hubContext;

        public CallServices(IHubContext<CallHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task JoinCallAsync(string roomName, string userId)
        {
            await _hubContext.Clients.Group(roomName).SendAsync("UserJoined", userId);
        }

        public async Task LeaveCallAsync(string roomName, string userId)
        {
            await _hubContext.Clients.Group(roomName).SendAsync("UserLeft", userId);
        }*/
    }
}
