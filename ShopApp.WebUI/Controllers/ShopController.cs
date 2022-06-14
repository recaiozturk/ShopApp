using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.WebUI.Models;
using System.Linq;

namespace ShopApp.WebUI.Controllers
{
    public class ShopController : Controller
    {

        private IProductService _productSevice;


        public ShopController(IProductService productSevice)
        {
            _productSevice = productSevice;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int? id)
        {
            if(id == 0)
            {
                return NotFound();
            }

            Product product = _productSevice.GetProductDetails ((int)id);

            if(product == null)
            {
                return NotFound();
            }

            return View(new ProductDetailsModel
            {
                Product = product,
                Categories=product.ProductCategories.Select(i=>i.Category).ToList()
            });
        }

        public IActionResult List()
        {
            return View(new ProductListModel()
            {
                Products = _productSevice.GetAll()
            });
        }
    }
}
