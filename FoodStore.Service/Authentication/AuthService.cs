using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Data.DTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace FoodStore.Service.Authentication
{
    public class AuthService : IAuthService
    {

         private readonly UserManager<ApplicationUser> _usermanager;
           private readonly JWT _jwt;
           private readonly IConfiguration _configuration;

         public AuthService(UserManager<ApplicationUser> usermanager,IOptions<JWT> jwt,IConfiguration configuration)
         {
            _usermanager = usermanager;
             _jwt = jwt.Value;
             _configuration = configuration;
         }




        public async  Task<AuthDto> LoggInAsync(LogInDto userDto)
        {
           
            var user = await _usermanager.FindByEmailAsync(userDto.Email);
            if(user is null || !await _usermanager.CheckPasswordAsync(user,userDto.password))
            {
                return new AuthDto{Message = "Password or Email is incorrect"};
            }

             var listRoles = await _usermanager.GetRolesAsync(user);
             var token = await CreateJwtToken(user);

             var authDto = new AuthDto();

            authDto.Email = userDto.Email;
            authDto.IsAuthenticated = true;
            authDto.Username = user.UserName;
            authDto.Roles =  listRoles.ToList();
            authDto.Token = new JwtSecurityTokenHandler().WriteToken(token);
            authDto.ExpiresOn = token.ValidTo;


            if(user.RefreshTokens.Any(t => t.IsActive) )
            {
                var refreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                authDto.RefreshToken = refreshToken.Token;
                authDto.RefreshTokenExpiration = refreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authDto.RefreshToken = refreshToken.Token;
                authDto.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _usermanager.UpdateAsync(user);
            }



           return authDto;


        }

        public async Task<AuthDto> RegisterAsync([FromBody]UserDto userDto)
        {
             if(await _usermanager.FindByEmailAsync(userDto.Email) is not null)
                return new AuthDto{Message = "Email is already registered!"};


                if(await _usermanager.FindByNameAsync(userDto.Username) is not null)
                return new AuthDto{Message = "Username is already registered!"};


                var user = new ApplicationUser{
                     Email = userDto.Email,
                     UserName = userDto.Username
                };
                
                

            var result = await _usermanager.CreateAsync(user,userDto.password);

            if(!result.Succeeded)
              {
                    var errors = string.Empty;
                    foreach (var error in result.Errors)
                    {
                        errors += $"{error.Description},";
                    }

                return new AuthDto{Message = errors};

              }


              await _usermanager.AddToRoleAsync(user,"Customer");

                    var jwtSecurityToken = await CreateJwtToken(user);

                    var refreshToken = GenerateRefreshToken();
                      user.RefreshTokens?.Add(refreshToken);
                      await _usermanager.UpdateAsync(user);



                return new AuthDto{
                    Email = userDto.Email,
                    Username = userDto.Username,
                    IsAuthenticated = true,
                    ExpiresOn = jwtSecurityToken.ValidTo,
                    Roles = new List<string> { "User" },
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    RefreshToken = refreshToken.Token,
                    RefreshTokenExpiration = refreshToken.ExpiresOn
                };
        }

       public async Task<AuthDto> RefreshTokenAsync(string token)
        {
            var authDto = new AuthDto();

            var user = await _usermanager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if(user == null)
            {
                authDto.Message = "Invalid token";
                return authDto;
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                authDto.Message = "Inactive token";
                return authDto;
            }

            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _usermanager.UpdateAsync(user);

            var jwtToken = await CreateJwtToken(user);
            authDto.IsAuthenticated = true;
            authDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authDto.Email = user.Email;
            authDto.Username = user.UserName;
            var roles = await _usermanager.GetRolesAsync(user);
            authDto.Roles = roles.ToList();
            authDto.RefreshToken = newRefreshToken.Token; 
            authDto.RefreshTokenExpiration = newRefreshToken.ExpiresOn; 

            return authDto;
        }

         public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _usermanager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.UtcNow;

            await _usermanager.UpdateAsync(user);

            return true;
        }




    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        // Get user claims and roles
        var userClaims = await _usermanager.GetClaimsAsync(user);
        var roles = await _usermanager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        foreach (var role in roles)
        {
            roleClaims.Add(new Claim(ClaimTypes.Role, role)); 
        }

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName), 
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
            new Claim(JwtRegisteredClaimNames.Email, user.Email), 
            new Claim(ClaimTypes.NameIdentifier, user.Id) 
        }
        .Union(userClaims)
        .Union(roleClaims);

        // Create the signing credentials
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        // Create the JWT token
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(int.Parse(_configuration["JWT:DurationInDays"])),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }






        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow
            };
        }
    }
}