using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Service.DTOS;

namespace FoodStore.Service.Authenticaion
{
    public interface IAuthService
    {
        Task<AuthDto> RegisterAsync(UserDto userDto);
        Task<AuthDto> LoggInAsync(LogInDto userDto);
        Task<AuthDto> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);

    }
}