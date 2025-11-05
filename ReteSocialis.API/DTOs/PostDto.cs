using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReteSocialis.API.DTOs
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string AuthorId { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}