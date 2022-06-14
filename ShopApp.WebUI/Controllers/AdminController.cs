using Microsoft.AspNetCore.Mvc;

namespace ShopApp.WebUI.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
