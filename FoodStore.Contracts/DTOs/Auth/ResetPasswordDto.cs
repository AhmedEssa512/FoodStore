using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FoodStore.Contracts.DTOs.Auth
{
    public class ResetPasswordDto
    {
        public string Email { get; set; } = default!;
        public string Token { get; set; } = default!;
        [JsonPropertyName("newPassword")]
        public string NewPassword { get; set; } = default!;
    }
}