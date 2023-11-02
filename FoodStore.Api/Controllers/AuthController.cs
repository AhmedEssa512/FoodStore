using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.DTOS;
using FoodStore.Service.Authenticaion;
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


        [HttpPost("RegisterAsync")]
        public async Task<IActionResult> RegisterAsync([FromBody]UserDto userDto)
        {
            if(!ModelState.IsValid)
             return BadRequest(ModelState);

              var result = await _authservice.RegisterAsync(userDto);

            if(!result.IsAuthenticated)
               return BadRequest(result.Message);


               SetRefreshTokenInCookie(result.RefreshToken,result.RefreshTokenExpiration);


               return Ok(result);
        }


        [HttpPost("LogInAsync")]
        public async Task<IActionResult> LogInAsync([FromBody]LogInDto userDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

                var result = await _authservice.LoggInAsync(userDto);

            if(!result.IsAuthenticated)
               return BadRequest(result.Message);

            if(! string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken,result.RefreshTokenExpiration);


               return Ok(result);
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _authservice.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }



         [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeToken dto)
        {
            var token = dto.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await _authservice.RevokeTokenAsync(token);

            if(!result)
                return BadRequest("Token is invalid!");

            return Ok("Revoked Succeeded");
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