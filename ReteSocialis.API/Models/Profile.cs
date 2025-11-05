using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReteSocialis.API.Models
{
    public class Profile
    {
        public int ProfileID { get; set; }
        public int AccountID { get; set; }
        public byte[]? Avatar { get; set; }
        public string? AvatarMimeType { get; set; }
        public bool UseGravatar { get; set; } = false;
    }
}