using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using FoodStore.Service.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authservice;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(IAuthService authservice , SignInManager<ApplicationUser> signInManager)
        {
            _authservice = authservice ;
            _signInManager = signInManager;
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]UserDto userDto)
        {
            if(!ModelState.IsValid)
             return BadRequest(ModelState);

              var result = await _authservice.RegisterAsync(userDto);

            if(!result.IsAuthenticated)
               return Unauthorized(new { message = result.Message });


               SetRefreshTokenInCookie(result.RefreshToken,result.RefreshTokenExpiration);


               return Ok(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> LogInAsync([FromBody]LogInDto userDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

                var result = await _authservice.LoggInAsync(userDto);

            if(!result.IsAuthenticated)
               return Unauthorized(new { message = result.Message });

            if(! string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken,result.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _authservice.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return Unauthorized(result);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }



         [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeToken dto)
        {
            var token = dto.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await _authservice.RevokeTokenAsync(token);

            if(!result)
                return BadRequest("Token is invalid!");

            return Ok();
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

         
    }
}