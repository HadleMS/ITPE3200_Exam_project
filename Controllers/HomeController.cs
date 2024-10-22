using Microsoft.AspNetCore.Mvc;

namespace MyShop.Controllers
{
    // HomeController class that inherits from Controller base class
    public class HomeController : Controller
    {
        // GET: /<controller>/ Lærer kommentert

        // Action method to handle GET requests to /Home or /
        public IActionResult Index()
        {
            // Returns a View
            return View();
        }
    }
}




