using Microsoft.EntityFrameworkCore;
using ReteSocialis.API.Data;
using ReteSocialis.API.Models;
using Microsoft.AspNetCore.SignalR;
using ReteSocialis.API.Hubs;

namespace ReteSocialis.API.Services
{
    public class FriendService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<FriendsHub> _hub;

        public FriendService(ApplicationDbContext context, IHubContext<FriendsHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        // 🔹 Enviar solicitação
        public async Task<bool> SendFriendRequestAsync(string senderId, string receiverEmail)
        {
            if (string.IsNullOrEmpty(receiverEmail) || string.IsNullOrEmpty(senderId))
                return false;

            var receiver = await _context.Users.FirstOrDefaultAsync(u => u.Email == receiverEmail);
            if (receiver == null) return false;

            var alreadyFriends = await _context.Friends.AnyAsync(f =>
                (f.UserId == senderId && f.FriendId == receiver.Id) ||
                (f.UserId == receiver.Id && f.FriendId == senderId));

            if (alreadyFriends) return false;

            _context.FriendInvitations.Add(new FriendInvitation
            {
                SenderId = senderId,
                ReceiverEmail = receiverEmail
            });

            await _context.SaveChangesAsync();

            await _hub.Clients.User(receiver.Id)
                .SendAsync("ReceiveFriendRequest", senderId);

            return true;
        }

        // 🔹 Aceitar solicitação
        public async Task<bool> AcceptFriendRequestAsync(Guid invitationKey, string receiverId)
        {
            var invitation = await _context.FriendInvitations
                .FirstOrDefaultAsync(i => i.InvitationKey == invitationKey);

            if (invitation == null || invitation.Accepted)
                return false;

            var sender = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == invitation.SenderId);

            if (sender == null) return false;

            _context.Friends.AddRange(
                new Friend { UserId = sender.Id, FriendId = receiverId },
                new Friend { UserId = receiverId, FriendId = sender.Id }
            );

            invitation.Accepted = true;
            await _context.SaveChangesAsync();

            await _hub.Clients.User(sender.Id)
                .SendAsync("FriendRequestAccepted", receiverId);

            return true;
        }

        // 🔹 Remover amigo
        public async Task RemoveFriendAsync(string userId, string friendId)
        {
            var relation = await _context.Friends
                .FirstOrDefaultAsync(f =>
                    (f.UserId == userId && f.FriendId == friendId) ||
                    (f.UserId == friendId && f.FriendId == userId));

            if (relation == null) return;

            _context.Friends.Remove(relation);
            await _context.SaveChangesAsync();

            await _hub.Clients.User(friendId)
                .SendAsync("FriendRemoved", userId);
        }

        // 🔹 Listar amigos
        public async Task<List<ApplicationUser>> GetFriendsAsync(string userId)
        {
            var friendIds = await _context.Friends
                .Where(f => f.UserId == userId)
                .Select(f => f.FriendId)
                .ToListAsync();

            return await _context.Users
                .Where(u => friendIds.Contains(u.Id))
                .ToListAsync();
        }

        
    }
}
