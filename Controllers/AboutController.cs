using Microsoft.AspNetCore.Mvc;

namespace Exam.Controllers
{

    // Controller to handle requests for the About section of the application.
    public class AboutController : Controller
    {

        public IActionResult Index()
        {
            // Returns a View
            return View();
        }
    }
}



