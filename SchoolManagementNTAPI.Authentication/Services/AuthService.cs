using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using SchoolManagementNTAPI.Authentication.Interfaces;
using SchoolManagementNTAPI.Authentication.JWT.Options;
using SchoolManagementNTAPI.Authentication.JWT;
using SchoolManagementNTAPI.Data.Entities;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using SchoolManagementNTAPI.Authentication.Requests;
using SchoolManagementNTAPI.Authentication.Responses;
using SchoolManagementNTAPI.Common.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SchoolManagementNTAPI.Authentication.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly JwtOptions _jwtOptions;
        private readonly JwtBearerOptions _jwtBearrerOptions;
        private readonly AspNetUserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AspNetRoleManager<IdentityRole> _roleManager;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly SchoolManagementNTDBContext _context;

        public AuthService(
            IJwtProvider jwtProvider,
            AspNetUserManager<IdentityUser> userManager,
            IOptions<JwtOptions> options,
            IOptions<JwtBearerOptions> jwtBearerOptions,
            SchoolManagementNTDBContext context,
            SignInManager<IdentityUser> signInManager,
            IRefreshTokenService refreshTokenService,
            AspNetRoleManager<IdentityRole> roleManager)
        {
            _jwtProvider = jwtProvider;
            _userManager = userManager;
            _jwtOptions = options.Value;
            _jwtBearrerOptions = jwtBearerOptions.Value;
            _context = context;
            _signInManager = signInManager;
            _refreshTokenService = refreshTokenService;
            _roleManager = roleManager;
        }

        public async Task<LogInResponse> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || user.Email is null)
            {
                return new LogInResponse { Message = "User does not exit." };
            }

            if (!await _signInManager.CanSignInAsync(user))
            {
                return new LogInResponse { Message = "User is not active." };
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);

            if (signInResult.IsLockedOut)
            {
                return new LogInResponse { Message = "User is locked out." };
            }

            if (!signInResult.Succeeded)
            {
                return new LogInResponse { Message = "Wrong credentials." };
            }

            await _refreshTokenService.DeleteRefreshTokensForUser(user.Id);

            HashSet<string> roles = new();
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var roleName in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);

                if (role != null)
                {
                    var claims = await _roleManager.GetClaimsAsync(role);
                    foreach (var claim in claims)
                    {
                        roles.Add(claim.Value);
                    }
                }
            }

            var student = await _context.Student.Include(x => x.StudentSubject).FirstAsync(x => x.AspNetUserId == user.Id);

            JWTModel JWTModel = new()
            {
                AspNetUserId = user.Id,
                AspNetEmail = user.Email,
                Roles = roles,
                StudentId = student.Id,
                Classes = student.StudentSubject.Select(x => x.SubjectId).ToList()
            };

            string authToken = _jwtProvider.GenerateToken(JWTModel);
            RefreshTokenDTO refreshToken = _jwtProvider.GenerateRefreshToken(JWTModel.AspNetUserId);

            await _refreshTokenService.SaveRefreshTokensForUser(refreshToken);

            LogInResponse authSuccessResponse = new()
            {
                Message = "Ok.",
                AuthToken = authToken,
                RefreshToken = refreshToken.Token
            };

            return authSuccessResponse;
        }

        public async Task<OperationStatusResponse> LogOut(LogOutRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || user.Email is null)
            {
                return new OperationStatusResponse { Message = "User does not exit." };
            }

            var rowsChanged = await _refreshTokenService.DeleteRefreshTokensForUser(user.Id);

            return new OperationStatusResponse { Message = $"Refresh tokens removed: {rowsChanged}." };
        }

        public async Task<RefreshTokenResponse> RefreshAuthToken(RefreshTokenRequest request)
        {
            var principal = GetPrincipalFromExpiredToken(request.AuthToken);

            if (principal is null)
                return new RefreshTokenResponse { Message = "Bad request." };

            var userClaim = principal.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub);
            var user = await _userManager.FindByIdAsync(userClaim.Value);

            if (user is null)
                return new RefreshTokenResponse { Message = "Bad request." };

            var refreshTokenExists = await _context.RefreshToken.AnyAsync(x => x.AspNetUserId == user.Id);

            if (!refreshTokenExists)
                return new RefreshTokenResponse { Message = "Refresh token not found." };

            var newAccessToken = _jwtProvider.RefreshToken(principal.Claims.ToList());

            return new RefreshTokenResponse { AuthToken = newAccessToken };
        }

        public async Task<OperationStatusResponse> ChangePassword(ChangePasswordUserRequest request)
        {
            if (request.NewPassword != request.ConfirmNewPassword)
                return new OperationStatusResponse { Message = "Лозинка и потврди лозинка не се совпаѓаат." };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return new OperationStatusResponse { Message = $"Не постои корисникот." };

            var canSignInResult = await _signInManager.CheckPasswordSignInAsync(user, request.CurrentPassword, true);

            if (!canSignInResult.Succeeded)
                return new OperationStatusResponse { Message = "Неточна mail или лозинка." };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            if (!result.Succeeded)
                return new OperationStatusResponse { Message = $"Serveer error: {request}" };

            await _refreshTokenService.DeleteRefreshTokensForUser(user.Id);

            return new OperationStatusResponse { Message = "Ok." };
        }

        public RefreshTokenRequest GetTokensFromRequest(HttpRequest request)
        {
            RefreshTokenRequest authTokens = new();
            request.Headers.TryGetValue("Authorization", out StringValues headerValue);

            authTokens.RefreshToken = request.Cookies["AppRefreshToken"];
            authTokens.AuthToken = headerValue.ToString().Split(' ')[1] ?? "";

            return authTokens;
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParams = _jwtBearrerOptions.TokenValidationParameters;

            tokenValidationParams.ValidateLifetime = false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParams, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
