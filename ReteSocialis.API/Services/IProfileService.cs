using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace ReteSocialis.API.Services
{
    public interface IProfileService
    {
        Task<dynamic?> GetProfileByUserAsync(ClaimsPrincipal user);
        Task SaveAvatarAsync(ClaimsPrincipal user, byte[] avatarBytes, string mimeType);
    }
}