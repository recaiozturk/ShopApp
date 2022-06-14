using Microsoft.AspNetCore.Mvc;

namespace ShopApp.WebUI.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
