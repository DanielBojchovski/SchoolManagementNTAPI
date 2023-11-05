using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SchoolManagementNTAPI.Authentication.Cookies;
using SchoolManagementNTAPI.Authentication.Interfaces;
using SchoolManagementNTAPI.Authentication.Requests;
using SchoolManagementNTAPI.Authentication.Responses;
using SchoolManagementNTAPI.Common.Responses;

namespace SchoolManagementNTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly DefaultCookieOptions _defaultCockieOptions;

        public AuthController(IAuthService authService, IOptions<DefaultCookieOptions> defaultCoockieOptions)
        {
            _authService = authService;
            _defaultCockieOptions = defaultCoockieOptions.Value;
        }

        [AllowAnonymous]
        [HttpPost("LogIn")]
        public async Task<ActionResult<LogInResponse>> LogIn(LoginRequest request)
        {
            HttpContext.Response.Cookies.Delete("AppRefreshToken");

            var response = await _authService.Login(request);

            if (response.RefreshToken != string.Empty)
                HttpContext.Response.Cookies.Append("AppRefreshToken",
                                                    response.RefreshToken,
                                                    _defaultCockieOptions.GetCookieOptions());

            return response;
        }


        [AllowAnonymous]
        [HttpGet("RefreshAuthToken")]
        public async Task<ActionResult<RefreshTokenResponse>> RefreshAuthToken()
        {
            var refreshTokenRequest = _authService.GetTokensFromRequest(Request);

            var response = await _authService.RefreshAuthToken(refreshTokenRequest);

            return response;
        }

        [HttpPost("LogOut")]
        public async Task<ActionResult<OperationStatusResponse>> LogOut(LogOutRequest request)
        {
            var response = await _authService.LogOut(request);

            return response;
        }

        [AllowAnonymous]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult<OperationStatusResponse>> ChangePassword(ChangePasswordUserRequest request)
        {
            return await _authService.ChangePassword(request);
        }
    }
}
