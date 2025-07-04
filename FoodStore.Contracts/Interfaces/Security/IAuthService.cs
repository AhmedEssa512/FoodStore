using FoodStore.Contracts.DTOs;
using FoodStore.Contracts.DTOs.Auth;

namespace FoodStore.Contracts.Interfaces.Security
{
    public interface IAuthService
    {
        Task<AuthResultWrapper<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task<AuthResultWrapper<LoginResponseDto>> LogInAsync(LoginRequestDto loginRequestDto);
        Task<AuthResultWrapper<RefreshTokenResponseDto>> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);

    }
}