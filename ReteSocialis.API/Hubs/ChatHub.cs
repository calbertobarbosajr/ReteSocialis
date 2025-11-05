using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ReteSocialis.API.Data;
using ReteSocialis.API.Models;

namespace ReteSocialis.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string receiverId, string message)
        {
            var senderId = Context.UserIdentifier;

            if (string.IsNullOrWhiteSpace(senderId) || string.IsNullOrWhiteSpace(receiverId))
                return;

            var msg = new Message
            {
                SenderId = int.Parse(senderId),
                ReceiverId = int.Parse(receiverId),
                Content = message
            };

            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();

            // Envia a mensagem para o destinatário e também para o remetente
            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message, msg.SentAt);
            await Clients.User(senderId).SendAsync("ReceiveMessage", senderId, message, msg.SentAt);
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"✅ Usuário conectado: {Context.UserIdentifier}");
            await base.OnConnectedAsync();
        }
    }
}