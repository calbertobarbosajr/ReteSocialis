

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ReteSocialis.API.Models;

namespace ReteSocialis.API.Hubs
{
    [Authorize]
    public class FeedHub : Hub
    {
        public async Task BroadcastNewPost(Post post)
        {
            await Clients.All.SendAsync("NewPost", post);
        }
    }
}