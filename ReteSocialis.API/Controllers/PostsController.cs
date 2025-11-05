
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ReteSocialis.API.Data;
using ReteSocialis.API.DTOs;
using ReteSocialis.API.Hubs;
using ReteSocialis.API.Models;

namespace ReteSocialis.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IHubContext<FeedHub> _hub;

        public PostsController(ApplicationDbContext db, IHubContext<FeedHub> hub)
        {
            _db = db;
            _hub = hub;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _db.Posts.OrderByDescending(p => p.CreatedAt).ToListAsync();
            var dtos = posts.Select(p => new PostDto
            {
                PostId = p.PostId,
                AuthorId = p.AuthorId,
                AuthorName = p.AuthorName,
                Content = p.Content,
                CreatedAt = p.CreatedAt
            });
            return Ok(dtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var p = await _db.Posts.FindAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostDto dto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userName = User.Identity?.Name ?? "Desconhecido";

            var post = new Post
            {
                AuthorId = userId ?? "",
                AuthorName = userName,
                Content = dto.Content
            };

            _db.Posts.Add(post);
            await _db.SaveChangesAsync();

            var postDto = new PostDto
            {
                PostId = post.PostId,
                AuthorId = post.AuthorId,
                AuthorName = post.AuthorName,
                Content = post.Content,
                CreatedAt = post.CreatedAt
            };

            // ✅ Notifica em tempo real via SignalR
            await _hub.Clients.All.SendAsync("NewPost", postDto);

            return CreatedAtAction(nameof(Get), new { id = post.PostId }, postDto);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PostDto dto)
        {
            var p = await _db.Posts.FindAsync(id);
            if (p == null) return NotFound();

            // opcional: checar se o usuário é autor do post
            p.Content = dto.Content;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Posts.FindAsync(id);
            if (p == null) return NotFound();
            _db.Posts.Remove(p);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}