using Microsoft.AspNetCore.Mvc;

namespace MyASPnet.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
