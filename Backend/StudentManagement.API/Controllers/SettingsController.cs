using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.BLL.Services.EmailService;

namespace StudentManagement.API.Controllers
{
    [Route("api/settings")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public SettingsController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpGet("email-domain")]
        public async Task<IActionResult> GetEmailDomain()
        {
            var result = await _emailService.GetEmailDomainAsync();
            return Ok(new { domain = result });    
        }
        
        [HttpPut("email-domain")]
        public async Task<IActionResult> SetEmailDomain([FromBody] string domain)
        {
            var result = await _emailService.SetEmailDomainAsync(domain);
            if (result)
            {
                return NoContent();
            }
            return BadRequest("Invalid domain format");
        }
       
    }
}
