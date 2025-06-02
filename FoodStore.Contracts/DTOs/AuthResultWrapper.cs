using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FoodStore.Contracts.DTOs
{
    public class AuthResultWrapper<T> where T : class
    {
        public T Response { get; set; } = default!;
        public string AccessToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiry { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiry { get; set; }
    }
}