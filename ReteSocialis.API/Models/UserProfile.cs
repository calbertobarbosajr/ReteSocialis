using System;
using System.ComponentModel.DataAnnotations;
using ReteSocialis.API.Data;

namespace ReteSocialis.API.Models
{
    public class UserProfile
    {
        // Usamos UserId como chave primária (mesma chave do ApplicationUser.Id)
        [Key]
        public string UserId { get; set; } = string.Empty;

        public byte[]? Avatar { get; set; }
        public string? AvatarMimeType { get; set; }

        public bool UseGravatar { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navegação para o usuário
        public ApplicationUser? User { get; set; }
    }
}