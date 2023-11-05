using Microsoft.AspNetCore.Mvc;
using SchoolManagementNTAPI.AppUser.Interfaces;
using SchoolManagementNTAPI.AppUser.Requests;
using SchoolManagementNTAPI.AppUser.Responses;
using SchoolManagementNTAPI.Common.Responses;

namespace SchoolManagementNTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<OperationStatusResponse>> CreateUser(CreateUserRequest request)
        {
            return await _userService.CreateUser(request);
        }

        [HttpPost("Update")]
        public async Task<ActionResult<OperationStatusResponse>> Update(UpdateUserRequest request)
        {
            return await _userService.Update(request);
        }

        [HttpGet("ListOfUsers")]
        public async Task<ActionResult<ListOfUsersResponse>> ListOfUsers()
        {
            return await _userService.ListOfUsers();
        }

        [HttpPost("GetUserById")]
        public async Task<ActionResult<GetUserByIdResponse>> GetUserById(IdUserRequest request)
        {
            return await _userService.GetUserById(request);
        }

        [HttpPost("ResetLockout")]
        public async Task<ActionResult<OperationStatusResponse>> ResetLockout(IdUserRequest request)
        {
            return await _userService.ResetLockout(request);
        }

        [HttpPost("ToogleUser")]
        public async Task<ActionResult<OperationStatusResponse>> ToogleUser(IdUserRequest request)
        {
            return await _userService.ToogleUser(request);
        }

        [HttpPost("ChangePassword")]
        public async Task<ActionResult<OperationStatusResponse>> ChangePassword(ChangePasswordRequest request)
        {
            return await _userService.ChangePassword(request);
        }

        [HttpGet("UsersDropDown")]
        public async Task<ActionResult<UserDropDownResponse>> UsersDropDown()
        {
            return await _userService.UsersDropDown();
        }
    }
}
