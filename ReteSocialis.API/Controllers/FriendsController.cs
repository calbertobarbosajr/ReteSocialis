using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReteSocialis.API.Services;
using System.Security.Claims;

namespace ReteSocialis.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FriendsController : ControllerBase
    {
        private readonly FriendService _friendService;

        public FriendsController(FriendService friendService)
        {
            _friendService = friendService;
        }

        // 🔹 GET /api/friends
        [HttpGet]
        public async Task<IActionResult> GetFriends()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var friends = await _friendService.GetFriendsAsync(userId);
            return Ok(friends.Select(f => new
            {
                f.Id,
                f.UserName,
                f.Email
            }));
        }

        // 🔹 POST /api/friends/invite
        [HttpPost("invite")]
        public async Task<IActionResult> InviteFriend([FromBody] InviteRequestDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var success = await _friendService.SendFriendRequestAsync(userId, dto.ReceiverEmail);
            if (!success) return BadRequest("Erro ao enviar convite.");

            return Ok(new { message = "Convite enviado!" });
        }

        // 🔹 PUT /api/friends/accept/{invitationKey}
        [HttpPut("accept/{invitationKey:guid}")]
        public async Task<IActionResult> AcceptInvitation(Guid invitationKey)
        {
            var receiverId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (receiverId == null) return Unauthorized();

            var success = await _friendService.AcceptFriendRequestAsync(invitationKey, receiverId);
            if (!success) return BadRequest("Convite inválido ou já aceito.");

            return Ok(new { message = "Convite aceito!" });
        }
    }

    // DTO para o convite
    public class InviteRequestDto
    {
        public string ReceiverEmail { get; set; } = string.Empty;
    }
}