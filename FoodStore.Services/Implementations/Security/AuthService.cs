using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FoodStore.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using FoodStore.Contracts.Config;
using FoodStore.Contracts.DTOs.Auth;
using AutoMapper;
using FoodStore.Contracts.DTOs;
using FoodStore.Services.Models;
using FoodStore.Contracts.Interfaces.Security;
using FoodStore.Contracts.Common;
using System.Net;
using FoodStore.Contracts.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FoodStore.Services.Exceptions;


namespace FoodStore.Services.Implementations.Security
{
    public class AuthService : IAuthService
    {

         private readonly UserManager<ApplicationUser> _userManager;
           private readonly JWT _jwt;
           private readonly IMapper _mapper;
           private readonly IEmailService _emailService;
           private readonly string _frontendBaseUrl;
           private readonly ILogger<AuthService> _logger;

         public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, IMapper mapper, IEmailService emailService, IConfiguration config,ILogger<AuthService> logger)
         {
            _userManager = userManager;
            _jwt = jwt.Value;
            _mapper = mapper;
            _emailService = emailService;
            _frontendBaseUrl = config["Frontend:BaseUrl"]!;
             _logger = logger;
         }


        public async Task<AuthResultWrapper<LoginResponseDto>> LogInAsync(LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);
            if (user is null)
            {
                return new AuthResultWrapper<LoginResponseDto>
                {
                    Response = new LoginResponseDto
                    {
                        IsAuthenticated = false,
                    }
                };
            }

            //  Check lockout / suspension
            if (user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                throw new OperationFailedException(
                    $"Your account is suspended until {user.LockoutEnd.Value.UtcDateTime}.");
            }
            

            if (!await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
            {
                return new AuthResultWrapper<LoginResponseDto>
                {
                    Response = new LoginResponseDto { IsAuthenticated = false }
                };
            }

            var listRoles = await _userManager.GetRolesAsync(user);
            var token = await CreateJwtToken(user);

            var authResult = new AuthResult
            {
                Email = loginRequestDto.Email,
                IsAuthenticated = true,
                Username = user.UserName,
                Roles = listRoles.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresOn = token.ValidTo
            };

            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var refreshToken = user.RefreshTokens.First(t => t.IsActive);
                authResult.RefreshToken = refreshToken.Token;
                authResult.RefreshTokenExpiration = refreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authResult.RefreshToken = refreshToken.Token;
                authResult.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            var responseDto = _mapper.Map<LoginResponseDto>(authResult);

            return new AuthResultWrapper<LoginResponseDto>
            {
                Response = responseDto,
                AccessToken = authResult.Token ?? string.Empty,
                AccessTokenExpiry = authResult.ExpiresOn,
                RefreshToken = authResult.RefreshToken ?? string.Empty,
                RefreshTokenExpiry = authResult.RefreshTokenExpiration
            };
        }

       
        public async Task<AuthResultWrapper<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(registerRequestDto.Email);
            if (existingUserByEmail is not null)
            {
                return new AuthResultWrapper<RegisterResponseDto>
                {
                    Response = new RegisterResponseDto
                    {
                        AccountCreated = false,
                        Message = "Email is already registered."
                    }
                };
            }

            var existingUserByName = await _userManager.FindByNameAsync(registerRequestDto.Username);
            if (existingUserByName is not null)
            {
                return new AuthResultWrapper<RegisterResponseDto>
                {
                    Response = new RegisterResponseDto
                    {
                        AccountCreated = false,
                        Message = "Username is already registered."
                    }
                };
            }

            var user = new ApplicationUser
            {
                Email = registerRequestDto.Email,
                UserName = registerRequestDto.Username,
                PhoneNumber = registerRequestDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, registerRequestDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthResultWrapper<RegisterResponseDto>
                {
                    Response = new RegisterResponseDto
                    {
                        AccountCreated = false,
                        Message = $"Registration failed: {errors}"
                    }
                };
            }

            await _userManager.AddToRoleAsync(user, "Customer");

            var jwtSecurityToken = await CreateJwtToken(user);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens?.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            // Build internal AuthResult
            var authResult = new AuthResult
            {
                Email = registerRequestDto.Email,
                Username = registerRequestDto.Username,
                IsAuthenticated = true,
                ExpiresOn = jwtSecurityToken.ValidTo,
                Roles = new List<string> { "Customer" },
                Token = tokenString,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresOn
            };

            var responseDto = _mapper.Map<RegisterResponseDto>(authResult);

            responseDto.Message = "Account created successfully.";

            return new AuthResultWrapper<RegisterResponseDto>
            {
                Response = responseDto,
                AccessToken = authResult.Token ?? string.Empty,
                AccessTokenExpiry = authResult.ExpiresOn,
                RefreshToken = authResult.RefreshToken ?? string.Empty,
                RefreshTokenExpiry = authResult.RefreshTokenExpiration
            };
        }

        public async Task<AuthResultWrapper<RefreshTokenResponseDto>> RefreshTokenAsync(string token)
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                return new AuthResultWrapper<RefreshTokenResponseDto>
                {
                    Response = new RefreshTokenResponseDto
                    {
                        IsAuthenticated = false,
                    }
                };
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                return new AuthResultWrapper<RefreshTokenResponseDto>
                {
                    Response = new RefreshTokenResponseDto
                    {
                        IsAuthenticated = false,
                    }
                };
            }

            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);

            await _userManager.UpdateAsync(user);

            var jwtToken = await CreateJwtToken(user);

            var roles = await _userManager.GetRolesAsync(user);

            var authResult = new AuthResult
            {
                IsAuthenticated = true,
                Email = user.Email,
                Username = user.UserName,
                Roles = roles.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                ExpiresOn = jwtToken.ValidTo,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiresOn,
            };

            var responseDto = _mapper.Map<RefreshTokenResponseDto>(authResult);

            return new AuthResultWrapper<RefreshTokenResponseDto>
            {
                Response = responseDto,
                AccessToken = authResult.Token ?? string.Empty,
                AccessTokenExpiry = authResult.ExpiresOn,
                RefreshToken = authResult.RefreshToken ?? string.Empty,
                RefreshTokenExpiry = authResult.RefreshTokenExpiration
            };
        }

         public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return true;
        }

        public Task<UserInfoDto> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            var username = user.Identity?.Name;
            var roles = user.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            var dto = new UserInfoDto
            {
                Email = email!,
                Username = username!,
                Roles = roles
            };

            return Task.FromResult(dto);
        }



    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        // Get user claims and roles
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        foreach (var role in roles)
        {
            roleClaims.Add(new Claim(ClaimTypes.Role, role)); 
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email!),           
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!), 
            new Claim(ClaimTypes.NameIdentifier, user.Id) 
        }
        .Union(userClaims)
        .Union(roleClaims);

        // Create the signing credentials
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        // Create the JWT token
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
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

        public async Task<OperationResult> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || string.IsNullOrWhiteSpace(user.Email))
              {
                return new OperationResult
                {
                    Success = true // for security
                };
              }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);

            var resetLink = $"{_frontendBaseUrl}/reset-password?email={user.Email}&token={encodedToken}";

            var subject = "Reset Your Password";
            var body =  $@"
                        <p>Hello {user.UserName},</p>
                        <p>Click the link below to reset your password:</p>
                        <p><a href=""{resetLink}"">{resetLink}</a></p>
                        <p><em>This link will expire in 15 minutes.</em></p>";
                        
            await _emailService.SendEmailAsync(user.Email, subject, body);

            return new OperationResult { Success = true };
        }

        public async Task<OperationResult> ResetPasswordAsync(string email, string token, string newPassword)
        {
            _logger.LogInformation("Reset password requested for email: {Email}", email);
            _logger.LogInformation("Encoded token: {Token}", token);

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User not found for email: {Email}", email);
                return new OperationResult
                {
                    Success = false,
                    Errors = new List<string> { "Invalid email." }
                };
            }


             var decodedToken = WebUtility.UrlDecode(token);
            _logger.LogInformation("token: {decodedToken}", decodedToken);
            _logger.LogInformation("New password: {NewPassword}", newPassword);


            var result = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError("Password reset error: {Code} - {Description}", error.Code, error.Description);
                }
            }

            return new OperationResult
            {
                Success = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }
    }
}