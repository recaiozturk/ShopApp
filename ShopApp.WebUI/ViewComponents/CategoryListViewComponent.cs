using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.Models;
using System.Threading.Tasks;

namespace ShopApp.WebUI.ViewComponents
{
    public class CategoryListViewComponent:ViewComponent
    {

        private IcategoryServicee _categoryService;

        public CategoryListViewComponent(IcategoryServicee categoryService)
        {
            _categoryService=categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(new CategoryListViewModel()
            {
                SelectedCategory=RouteData.Values["category"]?.ToString(),
                Categories = await _categoryService.GetAll()
            });
        }
    }
}
