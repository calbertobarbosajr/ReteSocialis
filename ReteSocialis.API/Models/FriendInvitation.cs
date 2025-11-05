using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReteSocialis.API.Data;

namespace ReteSocialis.API.Models
{
    public class FriendInvitation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Sender))]
        public string SenderId { get; set; } = string.Empty;

        public ApplicationUser? Sender { get; set; }

        [Required]
        [EmailAddress]
        public string ReceiverEmail { get; set; } = string.Empty;

        public Guid InvitationKey { get; set; } = Guid.NewGuid();

        public bool Accepted { get; set; } = false;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
