using Microsoft.AspNetCore.Mvc;
using SchoolManagementNTAPI.Authentication.Claims;
using SchoolManagementNTAPI.Authentication.JWT;
using SchoolManagementNTAPI.Notification.Interfaces;
using SchoolManagementNTAPI.Notification.Requests;
using SchoolManagementNTAPI.Notification.Responses;

namespace SchoolManagementNTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [HasClaim(UserClaims.User)]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _service;

        public EmailController(IEmailService service)
        {
            _service = service;
        }

        [HttpPost("SendEmail")]
        public async Task<ActionResult<SendEmailResponse>> SendEmail(SendEmailRequest request)
        {
            return await _service.SendEmail(request);
        }
    }
}
