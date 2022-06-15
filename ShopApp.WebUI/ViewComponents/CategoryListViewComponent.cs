using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.ViewComponents
{
    public class CategoryListViewComponent:ViewComponent
    {

        private IcategoryServicee _categoryService;

        public CategoryListViewComponent(IcategoryServicee categoryService)
        {
            _categoryService=categoryService;
        }

        public IViewComponentResult Invoke()
        {
            return View(new CategoryListViewModel()
            {
                SelectedCategory=RouteData.Values["category"]?.ToString(),
                Categories = _categoryService.GetAll()
            });
        }
    }
}
