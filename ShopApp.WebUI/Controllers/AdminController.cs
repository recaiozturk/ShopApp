using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.WebUI.Models;
using System.Linq;

namespace ShopApp.WebUI.Controllers
{
    public class AdminController : Controller
    {

        private IProductService _productService;
        private IcategoryServicee _categoryService;

        public AdminController(IProductService productService, IcategoryServicee categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        // List Products
        public IActionResult ProductList()
        {
            return View( new ProductListModel
            {
                Products=_productService.GetAll()
            });
        }

        //Edit Product - GET
        [HttpGet]
        public IActionResult EditProduct(int? id)
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
        public IActionResult EditProduct(ProductModel model)
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

            return RedirectToAction("ProductList");
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

            return RedirectToAction("ProductList");
        }

        //Delete Product
        [HttpPost]
        public IActionResult DeleteProduct(int productId)
        {
            var entity= _productService.GetById(productId);

            if (entity != null)
            {
                _productService.Delete(entity);
            }

            return RedirectToAction("ProductList");
        }


        //Category List 
        public IActionResult CategoryList()
        {
            return View(new CategoryListModel()
            {
                Categories = _categoryService.GetAll()
            }) ;
        }

        //Create Category - GET
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        //Create Category - POST
        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            var entity = new Category()
            {
                Name=model.Name,
            };

            _categoryService.Create(entity);

            return RedirectToAction("CategoryList");
        }

        //Edit Category - GET
        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            var entity=_categoryService.GetByIdWithProducts(id);

            return View(new CategoryModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Products = entity.ProductCategories.Select(p => p.Product).ToList()
            });
        }

        //Edit Category - POST
        [HttpPost]
        public IActionResult EditCategory(CategoryModel model)
        {
            var entity=_categoryService.GetById(model.Id);

            if(entity == null)
            {
                return NotFound();
            }

            entity.Name = model.Name;
            _categoryService.Update(entity);

            return RedirectToAction("CategoryList");
        }


        //Delete Category - POST
        [HttpPost]
        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);

            if (entity != null)
            {
                _categoryService.Delete(entity);
            }

            return RedirectToAction("CategoryList");
        }
    }

}
