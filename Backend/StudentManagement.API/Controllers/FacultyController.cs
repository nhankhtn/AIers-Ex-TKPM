using Microsoft.AspNetCore.Mvc;

namespace StudentManagement.API.Controllers
{
    public class FacultyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
