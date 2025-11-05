using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ReteSocialis.API.DTOs.Auth;
using ReteSocialis.API.Services;
using ReteSocialis.API.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ReteSocialis.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }

        // ----------- Registro -----------
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var existing = await _userManager.FindByNameAsync(model.Username);
            if (existing != null)
                return BadRequest("Username já em uso.");

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            var token = await _jwtTokenService.CreateTokenAsync(user);
            return Ok(new { token });
        }

        // ----------- Login -----------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null) return Unauthorized("Usuário não encontrado.");

            var passwordOk = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordOk) return Unauthorized("Senha incorreta.");

            var token = await _jwtTokenService.CreateTokenAsync(user);
            return Ok(new { token });
        }
    }
}
