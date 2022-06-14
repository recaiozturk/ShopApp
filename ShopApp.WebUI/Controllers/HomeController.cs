using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.Models;

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
            return View(new ProductListModel()
            {
                Products=_productSevice.GetAll()
            });
        }
    }
}
