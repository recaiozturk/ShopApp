using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.WebApi.DTO;
using System.Collections.Generic;
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

            //DTO sunmak istediğimiz data
            var productsDTO = new List<ProductDTO>();

            foreach (var product in products)
            {
                productsDTO.Add(ProductToDTO(product));
            }

            //Ok Status Code :200 Başarılı ile birlikte products gönderiyoruz
            return Ok(productsDTO);
        }


        // localhost:4200/api/products/2
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductsAsync(int id)
        {
            var product = await _productService.GetById(id);

            if(product == null)
                return NotFound();  //Kullanıcıya 404 hatası gider

            return Ok(ProductToDTO(product));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product entity)
        {
            await _productService.CreateAsync(entity);

            //name of la action isminin yanlış yazıp göndermenin önüne geçiyoruz
            return CreatedAtAction(nameof(GetProducts),new {id=entity.Id}, entity);  //201 Created kodu
        }

        //localhost:4200/api/products/2
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id,Product entity)
        {
            if( id != entity.Id)
            {
                return BadRequest();
            }

            var product = await _productService.GetById(id);

            if(product == null)
            {
                return BadRequest();
            }

            await _productService.UpdateAsync(entity);
            return NoContent();  //204 gider ama bilgilendirme gitmez
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product= await _productService.GetById(id);

            if(product == null)
            {
                return NotFound();
            }

            await _productService.DeleteAsync(product);

            return NoContent(); //204
        }

        //product i productDTO ya cevirir
        public static ProductDTO ProductToDTO(Product product)
        {
            return new ProductDTO
            {
                Name = product.Name,
                ProductId = product.Id,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Price = product.Price

            };
        }



    }
}
