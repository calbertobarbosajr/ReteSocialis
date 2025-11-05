using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace ReteSocialis.API.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<dynamic?> GetProfileByUserAsync(ClaimsPrincipal user)
        {
            var appUser = await _userManager.GetUserAsync(user);
            if (appUser == null) return null;

            // Aqui você buscaria os dados do perfil no banco
            return new { Username = appUser.UserName, Avatar = (byte[]?)null, AvatarMimeType = (string?)null };
        }

        public async Task SaveAvatarAsync(ClaimsPrincipal user, byte[] avatarBytes, string mimeType)
        {
            var appUser = await _userManager.GetUserAsync(user);
            if (appUser == null) throw new Exception("Usuário não encontrado.");

            // Exemplo de persistência (banco real ou storage)
            // Ex: _dbContext.UserProfiles.Update(avatarBytes...)

            await Task.CompletedTask; // simulação
        }
    }
}