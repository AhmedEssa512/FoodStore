using Microsoft.AspNetCore.Identity;

namespace FoodStore.Services.Seeds
{
    public class RoleSeeder
    {
        public async static Task SeedAsync(RoleManager<IdentityRole> _roleManager)
        {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name="Admin"
                });

                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name="Customer"
                });
        }
    }
}