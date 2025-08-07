using FoodStore.Contracts.DTOs.Auth;
using FoodStore.Contracts.Interfaces.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodStore.Contracts.DTOs;
using FoodStore.Contracts.Interfaces;
using FoodStore.Api.Helpers;
using FoodStore.Contracts.Common;


namespace FoodStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService ;
            _emailService = emailService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterRequestDto registerRequestDto)
        {
            if(!ModelState.IsValid)
             return BadRequest(ApiResponseHelper.FromModelState(ModelState));

            var result = await _authService.RegisterAsync(registerRequestDto);

            if(!result.Response.AccountCreated)
               return BadRequest(ApiResponse<string>.Fail(result.Response.Message ?? "Account creation failed."));

            if(!string.IsNullOrEmpty(result.AccessToken))
               SetAccessTokenInCookie(result.AccessToken, result.AccessTokenExpiry);

            if(!string.IsNullOrEmpty(result.RefreshToken))
               SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiry);

               return Ok(ApiResponse<RegisterResponseDto>.Ok(result.Response));
        }


        [HttpPost("login")]
        public async Task<IActionResult> LogInAsync([FromBody] LoginRequestDto loginRequestDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ApiResponseHelper.FromModelState(ModelState));

            var result = await _authService.LogInAsync(loginRequestDto);

            if(!result.Response.IsAuthenticated)
               return BadRequest(ApiResponse<string>.Fail("Email or password is incorrect."));

            if(!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiry);

            if(!string.IsNullOrEmpty(result.AccessToken))
               SetAccessTokenInCookie(result.AccessToken, result.AccessTokenExpiry);

            return Ok(ApiResponse<LoginResponseDto>.Ok(result.Response));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refresh_token"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized(ApiResponse<string>.Fail("Refresh token is missing."));
            
            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.Response.IsAuthenticated)
                return Unauthorized(ApiResponse<string>.Fail("Invalid or expired refresh token."));

            if(!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiry);
            
            if(!string.IsNullOrEmpty(result.AccessToken))
                SetAccessTokenInCookie(result.AccessToken, result.AccessTokenExpiry);
            
            return Ok(ApiResponse<RefreshTokenResponseDto>.Ok(result.Response));
        }



        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeToken dto)
        {
            var token = Request.Cookies["refresh_token"] ??  dto.Token  ;

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(ApiResponse<string>.Fail("Refresh token is required."));
            }

            var result = await _authService.RevokeTokenAsync(token);

            if(!result)
            {
                return BadRequest(ApiResponse<string>.Fail("Invalid or already revoked token."));
            }

            Response.Cookies.Delete("refresh_token", new CookieOptions
            {
                Path = "/",        
                Secure = true,     
                HttpOnly = true,   
                SameSite = SameSiteMode.None 
            });

            Response.Cookies.Delete("access_token", new CookieOptions
            {
                Path = "/",
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None
            });

            return Ok(ApiResponse<string>.Ok("Token revoked successfully."));
        }

        private void SetAccessTokenInCookie(string token, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                IsEssential = true,
                SameSite = SameSiteMode.None, 
                Expires = expires,
                Path = "/"
            };

            Response.Cookies.Append("access_token", token, cookieOptions);
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None,
                Path = "/"
            };

            Response.Cookies.Append("refresh_token", refreshToken, cookieOptions);
        }

        [HttpGet("is-authenticated")]
        [Authorize]
        public IActionResult IsAuthenticated()
        {
            return Ok(ApiResponse<bool>.Ok(true));
        } 

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _authService.GetCurrentUserAsync(User);
            return Ok(ApiResponse<UserInfoDto>.Ok(user)); 
        }
  

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            if (!ModelState.IsValid)
             return BadRequest(ApiResponseHelper.FromModelState(ModelState));

           var result = await _authService.ForgotPasswordAsync(request.Email);

            if (!result.Success)
             return BadRequest(ApiResponse<string>.Fail("Unable to process the password reset request."));
            
            return Ok(ApiResponse<string>.Ok("If this email exists, a reset link has been sent."));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            var result = await _authService.ResetPasswordAsync(
                request.Email,
                request.Token,
                request.NewPassword
            );

           if (!result.Success)
            return BadRequest(ApiResponse<List<string>>.Fail("Password reset failed."));

            return Ok(ApiResponse<string>.Ok("Password has been reset successfully."));
        }
    }
}