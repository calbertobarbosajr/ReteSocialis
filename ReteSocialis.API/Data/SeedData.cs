using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// ReteSocialis.API/Data/SeedData.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ReteSocialis.API.Data
{
    public static  class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await context.Database.MigrateAsync();

            if (!userManager.Users.Any())
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@local",
                    AvatarUrl = null
                };

                await userManager.CreateAsync(admin, "Admin@123");
            }
        }
    }
}