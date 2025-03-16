using Microsoft.AspNetCore.Mvc;
using StudentManagement.BLL.DTOs;

namespace StudentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacultyController : Controller
    {
        [HttpPost()]
        public async Task<ActionResult<FacultyDTO>> AddFaculty(string name)
        {
            return NoContent();
        }
    }
}
