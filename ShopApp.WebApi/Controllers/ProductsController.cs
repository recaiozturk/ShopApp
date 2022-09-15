using Microsoft.AspNetCore.Mvc;

namespace ShopApp.WebApi.Controllers
{
    // localhost:4200/api/products
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static readonly string[] Products =
        {
            "samsung S1",
            "samsung S2",
            "samsung S3",
            "samsung S4"
        };

        [HttpGet]
        public string[] GetProducts()
        {
            return Products;
        }


        // localhost:4200/api/products/2
        [HttpGet("{id}")]
        public string GetProducts(int id)
        {
            return Products[id];
        }
    }
}
