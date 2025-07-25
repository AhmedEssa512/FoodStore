using System.Security.Claims;
using FoodStore.Contracts.Common;
using FoodStore.Contracts.DTOs;
using FoodStore.Contracts.DTOs.Auth;

namespace FoodStore.Contracts.Interfaces.Security
{
    public interface IAuthService
    {
        Task<AuthResultWrapper<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task<AuthResultWrapper<LoginResponseDto>> LogInAsync(LoginRequestDto loginRequestDto);
        Task<AuthResultWrapper<RefreshTokenResponseDto>> RefreshTokenAsync(string refreshToken);
        Task<UserInfoDto> GetCurrentUserAsync(ClaimsPrincipal user);
        Task<bool> RevokeTokenAsync(string refreshToken);
        Task<OperationResult> ForgotPasswordAsync(string email);
        Task<OperationResult> ResetPasswordAsync(string email, string token, string newPassword);
    }
}