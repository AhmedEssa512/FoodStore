using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;

namespace FoodStore.Service.Authentication
{
    public interface IAuthService
    {
        Task<AuthDto> RegisterAsync(UserDto userDto);
        Task<AuthDto> LogInAsync(LogInDto userDto);
        Task<AuthDto> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);

    }
}