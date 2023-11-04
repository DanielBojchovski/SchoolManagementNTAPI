using Microsoft.AspNetCore.Mvc;
using SchoolManagementNTAPI.Common.Requests;
using SchoolManagementNTAPI.Common.Responses;
using SchoolManagementNTAPI.Subject.Interfaces;
using SchoolManagementNTAPI.Subject.Models;
using SchoolManagementNTAPI.Subject.Requests;
using SchoolManagementNTAPI.Subject.Responses;

namespace SchoolManagementNTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _service;

        public SubjectController(ISubjectService service)
        {
            _service = service;
        }

        [HttpGet("GetAllSubjects")]
        public async Task<ActionResult<GetAllSubjectsResponse>> GetAllSubjects()
        {
            return await _service.GetAllSubjects();
        }

        [HttpPost("GetSubjectById")]
        public async Task<ActionResult<SubjectModel?>> GetSubjectById(IdRequest request)
        {
            var response = await _service.GetSubjectById(request);
            return response == null ? NotFound() : response;
        }

        [HttpPost("CreateSubject")]
        public async Task<ActionResult<OperationStatusResponse>> CreateSubject(CreateSubjectRequest request)
        {
            return await _service.CreateSubject(request);
        }

        [HttpPost("UpdateSubject")]
        public async Task<ActionResult<OperationStatusResponse>> UpdateSubject(UpdateSubjectRequest request)
        {
            return await _service.UpdateSubject(request);
        }

        [HttpPost("DeleteSubject")]
        public async Task<ActionResult<OperationStatusResponse>> DeleteSubject(IdRequest request)
        {
            return await _service.DeleteSubject(request);
        }
    }
}
