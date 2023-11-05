using Microsoft.AspNetCore.Mvc;
using SchoolManagementNTAPI.Authentication.Claims;
using SchoolManagementNTAPI.Common.Requests;
using SchoolManagementNTAPI.Common.Responses;
using SchoolManagementNTAPI.Professor.Interfaces;
using SchoolManagementNTAPI.Professor.Models;
using SchoolManagementNTAPI.Professor.Requests;
using SchoolManagementNTAPI.Professor.Responses;

namespace SchoolManagementNTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [HasClaim(UserClaims.User)]
    public class ProfessorController : ControllerBase
    {
        private readonly IProfessorService _service;

        public ProfessorController(IProfessorService service)
        {
            _service = service;
        }

        [HttpGet("GetAllProfessors")]
        public async Task<ActionResult<GetAllProfessorsResponse>> GetAllProfessors()
        {
            return await _service.GetAllProfessors();
        }

        [HttpPost("GetProfessorById")]
        public async Task<ActionResult<ProfessorModel?>> GetProfessorById(IdRequest request)
        {
            var response = await _service.GetProfessorById(request);
            return response == null ? NotFound() : response;
        }

        [HttpPost("GetProfessorsBySchoolId")]
        public async Task<ActionResult<GetAllProfessorsResponse>> GetProfessorsBySchoolId(IdRequest request)
        {
            return await _service.GetProfessorsBySchoolId(request);
        }

        [HttpPost("CreateProfessor")]
        public async Task<ActionResult<OperationStatusResponse>> CreateProfessor(CreateProfessorRequest request)
        {
            return await _service.CreateProfessor(request);
        }

        [HttpPost("UpdateProfessor")]
        public async Task<ActionResult<OperationStatusResponse>> UpdateProfessor(UpdateProfessorRequest request)
        {
            return await _service.UpdateProfessor(request);
        }

        [HttpPost("DeleteProfessor")]
        public async Task<ActionResult<OperationStatusResponse>> DeleteProfessor(IdRequest request)
        {
            return await _service.DeleteProfessor(request);
        }
    }
}
