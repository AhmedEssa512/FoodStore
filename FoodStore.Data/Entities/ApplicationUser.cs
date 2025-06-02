using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace FoodStore.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public  List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}