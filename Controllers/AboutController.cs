using Microsoft.AspNetCore.Mvc;

namespace MyShop.Controllers
{
    public class AboutController : Controller
    {
       
        public IActionResult About()
        {
            // Returns a View
            return View();
        }
    }
}



