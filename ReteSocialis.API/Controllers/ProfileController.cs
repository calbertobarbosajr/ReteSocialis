using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReteSocialis.API.Models;
using ReteSocialis.API.Services;

namespace ReteSocialis.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var profile = await _profileService.GetProfileByUserAsync(User);
            if (profile == null) return NotFound();

            var dto = new ProfileDto
            {
                Username = profile.Username,
                AvatarBase64 = profile.Avatar != null
                    ? $"data:{profile.AvatarMimeType};base64,{Convert.ToBase64String(profile.Avatar)}"
                    : null
            };

            return Ok(dto);
        }

        [HttpPost("avatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile avatar)
        {
            if (avatar == null)
                return BadRequest(new { message = "Nenhum arquivo recebido." });

            var validExts = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var ext = Path.GetExtension(avatar.FileName).ToLower();

            if (!validExts.Contains(ext))
                return BadRequest(new { message = "Formato inválido. Use JPG, PNG ou GIF." });

            if (avatar.Length > 1024 * 1024)
                return BadRequest(new { message = "A imagem deve ter no máximo 1MB." });

            using var ms = new MemoryStream();
            await avatar.CopyToAsync(ms);

            await _profileService.SaveAvatarAsync(User, ms.ToArray(), avatar.ContentType);

            return Ok(new { message = "Avatar atualizado com sucesso!" });
        }
    }
}