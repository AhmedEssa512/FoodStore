using FoodStore.Contracts.DTOs.Auth;
using FoodStore.Contracts.Interfaces.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FoodStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authservice;

        public AuthController(IAuthService authservice)
        {
            _authservice = authservice ;
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterRequestDto registerRequestDto)
        {
            if(!ModelState.IsValid)
             return BadRequest(ModelState);

            var result = await _authservice.RegisterAsync(registerRequestDto);

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

            var result = await _authservice.LogInAsync(loginRequestDto);

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

            var result = await _authservice.RefreshTokenAsync(refreshToken);

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

            var result = await _authservice.RevokeTokenAsync(token);

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
    }
}