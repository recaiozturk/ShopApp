using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;

namespace ShopApp.WebApi.Controllers
{
    // localhost:4200/api/products
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }



        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _productService.GetAll();

            //Ok Status Code :200 Başarılı ile birlikte products gönderiyoruz
            return Ok(products);
        }


        // localhost:4200/api/products/2
        [HttpGet("{id}")]
        public IActionResult GetProducts(int id)
        {
            var product = _productService.GetById(id);

            if(product == null)
                return NotFound();  //Kullanıcıya 404 hatası gider

            return Ok(product);
        }
    }
}
