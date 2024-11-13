using Microsoft.AspNetCore.Mvc;

namespace MyShop.Controllers
{
    public class AboutController : Controller
    {
       
        public IActionResult Index()
        {
            // Returns a View
            return View();
        }
    }
}



