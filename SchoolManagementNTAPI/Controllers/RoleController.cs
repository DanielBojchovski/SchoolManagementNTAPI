using Microsoft.AspNetCore.Mvc;
using SchoolManagementNTAPI.Common.Responses;
using SchoolManagementNTAPI.Role.Interfaces;
using SchoolManagementNTAPI.Role.Requests;
using SchoolManagementNTAPI.Role.Responses;

namespace SchoolManagementNTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRolesService _roleService;

        public RoleController(IRolesService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("CreateRole")]
        public async Task<ActionResult<OperationStatusResponse>> CreateRole(CreateRoleRequest request)
        {
            return await _roleService.CreateRole(request);
        }

        [HttpPost("UpdateRole")]
        public async Task<ActionResult<OperationStatusResponse>> UpdateRole(UpdateRoleRequest request)
        {
            return await _roleService.UpdateRole(request);
        }

        [HttpGet("GetRoles")]
        public async Task<ActionResult<GetRolesResponse>> GetRoles()
        {
            return await _roleService.GetRoles();
        }

        [HttpPost("GetRoleById")]
        public async Task<ActionResult<GetRoleByIdResponse>> GetRoleById(IdRoleRequest request)
        {
            return await _roleService.GetRoleById(request);
        }

        [HttpPost("DeleteRole")]
        public async Task<ActionResult<OperationStatusResponse>> DeleteRole(IdRoleRequest request)
        {
            return await _roleService.DeleteRole(request);
        }

        [HttpGet("GetAllClaims")]
        public ActionResult<GetAllClaimsResponse> GetAllClaims()
        {
            return _roleService.GetAllClaims();
        }

        [HttpGet("GetRolesDropDown")]
        public async Task<ActionResult<GetRolesDropDownResponse>> GetRolesDropDown()
        {
            return await _roleService.GetRolesDropDown();
        }
    }
}
