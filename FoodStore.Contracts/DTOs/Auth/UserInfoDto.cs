using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Auth
{
    public class UserInfoDto 
    {
        public string Email { get; set; } = default!;
        public string Username { get; set; } = default!;
        public List<string>? Roles { get; set; }
    }
}