using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;

namespace ShopApp.WebUI.Controllers
{
    public class HomeController : Controller
    {

        private IProductService _productSevice;

        public HomeController(IProductService productSevice)
        {
            _productSevice = productSevice;
        }

        public IActionResult Index()
        {
            return View(_productSevice.GetAll());
        }
    }
}
