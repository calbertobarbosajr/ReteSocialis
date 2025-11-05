using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// ReteSocialis.API/Data/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;
using ReteSocialis.API.Models;

namespace ReteSocialis.API.Data
{
    public class ApplicationUser : IdentityUser
    {
        // Campo opcional para armazenar a URL do avatar
        public string? AvatarUrl { get; set; }

        // Data de criação do perfil
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public UserProfile? Profile { get; set; }
    }
}