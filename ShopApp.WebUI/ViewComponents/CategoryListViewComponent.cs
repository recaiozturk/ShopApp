using Microsoft.AspNetCore.Mvc;

namespace ShopApp.WebUI.ViewComponents
{
    public class CategoryListViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
