using Microsoft.AspNetCore.Mvc;

namespace Exam.Controllers
{

    // Controller to handle requests for the Home section of the application.
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            // Returns a View
            return View();
        }
    }


}




