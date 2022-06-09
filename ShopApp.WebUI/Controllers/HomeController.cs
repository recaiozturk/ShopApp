using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    public class HomeController : Controller
    {

        private IProductService _productSevice;
        private IcategoryServicee _categorySevice;

        public HomeController(IProductService productSevice, IcategoryServicee categorySevice)
        {
            _productSevice = productSevice;
            _categorySevice=categorySevice;
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
