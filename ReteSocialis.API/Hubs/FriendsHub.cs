using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ReteSocialis.API.Hubs
{
    public class FriendsHub : Hub
    {
        public async Task SendFriendRequest(string toUserId)
        {
            await Clients.User(toUserId).SendAsync("ReceiveFriendRequest");
        }
    }
}