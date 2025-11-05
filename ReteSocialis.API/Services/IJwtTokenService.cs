
using Microsoft.AspNetCore.Identity;
using ReteSocialis.API.Data;

namespace ReteSocialis.API.Services
{
    public interface IJwtTokenService
    {
        Task<string> CreateTokenAsync(ApplicationUser user);
    }

}