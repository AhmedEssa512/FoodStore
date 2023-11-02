using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FoodStore.Service.Seeds
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