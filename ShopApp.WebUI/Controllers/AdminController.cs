using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.WebUI.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    [Authorize(Roles ="admin")]
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
        public async Task<IActionResult> ProductList()
        {
            var products= await _productService.GetAll();

            return View( new ProductListModel
            {
                Products=products
            });
        }

        //Edit Product - GET
        [HttpGet]
        public async Task<IActionResult> EditProductAsync(int? id)
        {
            if(id== null)
            {
                return NotFound();
            }

            var entity=_productService.GetByIdWithCategories((int)id);

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
                ImageUrl = entity.ImageUrl,
                SelectedCategories=entity.ProductCategories.Select(c=>c.Category).ToList(),
            };

            //sayfaya aynı zmanda tm categorileri de gönderelim
            ViewBag.Categories= await _categoryService.GetAll();

            return View(model);
        }

        //Edit Product - POST
        [HttpPost]
        public async Task<IActionResult> EditProductAsync(ProductModel model, int[] categoryIds,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var entity = await _productService.GetById(model.Id);

                if (entity == null)
                {
                    return NotFound();
                }

                entity.Name = model.Name;
                entity.Price = model.Price;
                entity.Description = model.Description;

                //gelen esim dosyası boş değil ise
                if (file != null)
                {
                    entity.ImageUrl = file.FileName;

                    //filename i random olarak ayarlayabiliriz daha sonra
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);

                    using(var stream = new FileStream(path, FileMode.Create))
                    {
                         file.CopyToAsync(stream);
                    }
                }
                

                _productService.Update(entity, categoryIds);

                return RedirectToAction("ProductList");
            }


            //sayfaya aynı zmanda tm categorileri de gönderelim
            ViewBag.Categories =  await _categoryService.GetAll();

            return View(model);
            
        }

        //Create Product - GET
        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View(new ProductModel());
        }

        //Create Product - POST
        [HttpPost]
        public IActionResult CreateProduct(ProductModel model)
        {

            if (ModelState.IsValid)
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

            return View(model);

            
        }

        //Delete Product
        [HttpPost]
        public async Task<IActionResult> DeleteProductAsync(int productId)
        {
            var entity=  await _productService.GetById(productId);

            if (entity != null)
            {
                _productService.Delete(entity);
            }

            return RedirectToAction("ProductList");
        }


        //Category List 
        public async Task<IActionResult> CategoryList()
        {
            return View(new CategoryListModel()
            {
                Categories = await _categoryService.GetAll()
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
        public async Task<IActionResult> EditCategory(CategoryModel model)
        {
            var entity= await _categoryService.GetById(model.Id);

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
        public async Task<IActionResult> DeleteCategoryAsync(int categoryId)
        {
            var entity =  await _categoryService.GetById(categoryId);

            if (entity != null)
            {
                _categoryService.Delete(entity);
            }

            return RedirectToAction("CategoryList");
        }


        //Delete From Category
        [HttpPost]
        public IActionResult DeleteFromCategory(int categoryId,int productId)
        {
            _categoryService.DeleteFromCategory(categoryId, productId);

            return Redirect("/admin/editcategory/"+categoryId);
        }
    }

}
