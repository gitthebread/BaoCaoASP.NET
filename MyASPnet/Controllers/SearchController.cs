using Microsoft.AspNetCore.Mvc;

namespace MyASPnet.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
