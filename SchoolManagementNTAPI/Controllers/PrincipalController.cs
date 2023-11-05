using Microsoft.AspNetCore.Mvc;
using SchoolManagementNTAPI.Authentication.Claims;
using SchoolManagementNTAPI.Common.Requests;
using SchoolManagementNTAPI.Common.Responses;
using SchoolManagementNTAPI.Principal.Interfaces;
using SchoolManagementNTAPI.Principal.Models;
using SchoolManagementNTAPI.Principal.Requests;
using SchoolManagementNTAPI.Principal.Responses;

namespace SchoolManagementNTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [HasClaim(UserClaims.User)]
    public class PrincipalController : ControllerBase
    {
        private readonly IPrincipalService _service;

        public PrincipalController(IPrincipalService service)
        {
            _service = service;
        }

        [HttpGet("GetAllPrincipals")]
        public async Task<ActionResult<GetAllPrincipalsResponse>> GetAllPrincipals()
        {
            return await _service.GetAllPrincipals();
        }

        [HttpPost("GetPrincipalById")]
        public async Task<ActionResult<PrincipalModel?>> GetPrincipalById(IdRequest request)
        {
            var response = await _service.GetPrincipalById(request);
            return response == null ? NotFound() : response;
        }

        [HttpPost("GetPrincipalBySchoolId")]
        public async Task<ActionResult<PrincipalModel?>> GetPrincipalBySchoolId(IdRequest request)
        {
            var response = await _service.GetPrincipalBySchoolId(request);
            return response == null ? NotFound() : response;
        }

        [HttpPost("CreatePrincipal")]
        public async Task<ActionResult<OperationStatusResponse>> CreatePrincipal(CreatePrincipalRequest request)
        {
            return await _service.CreatePrincipal(request);
        }

        [HttpPost("UpdatePrincipal")]
        public async Task<ActionResult<OperationStatusResponse>> UpdatePrincipal(UpdatePrincipalRequest request)
        {
            return await _service.UpdatePrincipal(request);
        }

        [HttpPost("DeletePrincipal")]
        public async Task<ActionResult<OperationStatusResponse>> DeletePrincipal(IdRequest request)
        {
            return await _service.DeletePrincipal(request);
        }
    }
}
