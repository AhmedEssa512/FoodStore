using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Auth
{
    public class RevokeToken
    {
        public string? Token { get; set; }
    }
}