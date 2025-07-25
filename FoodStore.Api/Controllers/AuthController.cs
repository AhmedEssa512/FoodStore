using FoodStore.Contracts.DTOs.Auth;
using FoodStore.Contracts.Interfaces.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodStore.Contracts.DTOs;
using FoodStore.Contracts.Interfaces;


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
             return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(registerRequestDto);

            if(!result.Response.AccountCreated)
               return Unauthorized(result.Response.Message);

            if(!string.IsNullOrEmpty(result.AccessToken))
               SetAccessTokenInCookie(result.AccessToken, result.AccessTokenExpiry);

            if(!string.IsNullOrEmpty(result.RefreshToken))
               SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiry);

               return Ok(result.Response);
        }


        [HttpPost("login")]
        public async Task<IActionResult> LogInAsync([FromBody] LoginRequestDto loginRequestDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LogInAsync(loginRequestDto);

            if(!result.Response.IsAuthenticated)
               return Unauthorized(new { message = "Password or email is incorrect" });

            if(!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiry);

            if(!string.IsNullOrEmpty(result.AccessToken))
               SetAccessTokenInCookie(result.AccessToken, result.AccessTokenExpiry);

            return Ok(result.Response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refresh_token"];

            if(refreshToken == null)
                return Unauthorized();

            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.Response.IsAuthenticated)
                return Unauthorized(new { message = "Invalid token" });

            if(!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiry);
            
            if(!string.IsNullOrEmpty(result.AccessToken))
                SetAccessTokenInCookie(result.AccessToken, result.AccessTokenExpiry);
            
            return Ok(result.Response);
        }



        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeToken dto)
        {
            var token = Request.Cookies["refresh_token"] ??  dto.Token  ;

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is required!" });
            }

            var result = await _authService.RevokeTokenAsync(token);

            if(!result)
            {
                return BadRequest(new { message = "Invalid token" });
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

            return Ok();
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
            return Ok(new { IsAuthenticated = true });
        } 

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _authService.GetCurrentUserAsync(User);
            return Ok(user); 
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] MailRequest request)
        {
            await _emailService.SendEmailAsync(request.To, request.Subject, request.Body);
            return Ok("Email sent successfully!");
        }    

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
           var result = await _authService.ForgotPasswordAsync(request.Email);

             if (!result.Success)
                return BadRequest(new { errors = result.Errors });
            
            return Ok(new { message = "A reset link has been sent the email." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            var result = await _authService.ResetPasswordAsync(
                request.Email,
                request.Token,
                request.NewPassword
            );

          //  if (!result.Success)
          //      return BadRequest(new { errors = result.Errors });

            return Ok(result);
        }
    }
}