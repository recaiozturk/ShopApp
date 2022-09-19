using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetProducts()
        {
            var products =  await _productService.GetAll();

            //Ok Status Code :200 Başarılı ile birlikte products gönderiyoruz
            return Ok(products);
        }


        // localhost:4200/api/products/2
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductsAsync(int id)
        {
            var product = await _productService.GetById(id);

            if(product == null)
                return NotFound();  //Kullanıcıya 404 hatası gider

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product entity)
        {
            await _productService.CreateAsync(entity);

            //name of la action isminin yanlış yazıp göndermenin önüne geçiyoruz
            return CreatedAtAction(nameof(GetProducts),new {id=entity.Id}, entity);  //201 Created kodu
        }
    }
}
