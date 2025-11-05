using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReteSocialis.API.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string AuthorId { get; set; } = string.Empty; // FK para IdentityUser.Id
        public string AuthorName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}