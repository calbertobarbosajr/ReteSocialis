using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReteSocialis.API.Models
{
    public class ProfileDto
    {
        public string Username { get; set; } = "";
        public string? AvatarBase64 { get; set; }
    }
}