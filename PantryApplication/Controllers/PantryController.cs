using Microsoft.AspNetCore.Mvc;

namespace PantryApplication.Controllers
{
    public class PantryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
