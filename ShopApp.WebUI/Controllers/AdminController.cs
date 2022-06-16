using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    public class AdminController : Controller
    {

        private IProductService _productService;

        public AdminController(IProductService productService)
        {
            _productService = productService;
        }

        //Index - List Products
        public IActionResult Index()
        {
            return View( new ProductListModel
            {
                Products=_productService.GetAll()
            });
        }



        //Edit Product - GET
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id== null)
            {
                return NotFound();
            }

            var entity=_productService.GetById((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model= new ProductModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl
            };

            return View(model);
        }

        //Edit Product - POST
        [HttpPost]
        public IActionResult Edit(ProductModel model)
        {

            var entity=_productService.GetById(model.Id);

            if(entity == null)
            {
                return NotFound();
            }

            entity.Name = model.Name;
            entity.Price = model.Price;
            entity.Description = model.Description;
            entity.ImageUrl = model.ImageUrl;

            _productService.Update(entity);

            return RedirectToAction("Index");
        }

        //Create Product - GET
        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }


        //Create Product - POST
        [HttpPost]
        public IActionResult CreateProduct(ProductModel model)
        {
            var entity = new Product()
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                ImageUrl = model.ImageUrl

            };

            _productService.Create(entity);

            return RedirectToAction("Index");
        }

        //Delete Product
        [HttpPost]
        public IActionResult Delete(int productId)
        {
            var entity= _productService.GetById(productId);

            if (entity != null)
            {
                _productService.Delete(entity);
            }

            return RedirectToAction("Index");
        }
    }

}
