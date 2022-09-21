using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.WebUI.Models;
using System.Collections.Generic;
using System.Net.Http;
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

        //Api Test için
        public async Task<IActionResult> GetProductsFromRestAPI()
        {
            var products = new List<Product>();

            //işimiz bittikten sorna bellekten silinsin
            using(var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:4200/api/products/"))
                {
                    string apiResponse =await response.Content.ReadAsStringAsync();
                    products=JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                }
            }
            return View(products);
        }
    }
}
