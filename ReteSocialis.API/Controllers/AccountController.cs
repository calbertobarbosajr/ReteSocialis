using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReteSocialis.API.Data;
using ReteSocialis.API.Models;
using System.Security.Claims;

namespace ReteSocialis.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // 🔹 GET /api/account/me
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("Usuário não autenticado.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Usuário não encontrado.");

            return Ok(new
            {
                user.UserName,
                user.Email
            });
        }

        // 🔹 PUT /api/account/edit
        [HttpPut("edit")]
        public async Task<IActionResult> EditAccount([FromBody] EditAccountDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            // ✅ Atualiza nome e e-mail
            user.UserName = model.Username;
            user.Email = model.Email;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return BadRequest(updateResult.Errors);

            // ✅ Atualiza senha (caso informado)
            if (!string.IsNullOrWhiteSpace(model.OldPassword) && !string.IsNullOrWhiteSpace(model.NewPassword))
            {
                var passResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!passResult.Succeeded)
                    return BadRequest(passResult.Errors);
            }

            return Ok(new { message = "Conta atualizada com sucesso!" });
        }
    }

    public class EditAccountDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
