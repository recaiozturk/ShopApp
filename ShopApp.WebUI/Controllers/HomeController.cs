using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.Models;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    public class HomeController : Controller
    {

        private IProductService _productSevice;
        

        public HomeController(IProductService productSevice)
        {
            _productSevice = productSevice;
            
        }

        public  async Task<IActionResult> Index()
        {
            var products = await _productSevice.GetAll();
            return View(new ProductListModel()
            {
                Products= products
            });
        }
    }
}
