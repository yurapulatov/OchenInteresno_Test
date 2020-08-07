using Microsoft.AspNetCore.Mvc;

namespace BadBroker.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}