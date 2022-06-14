using ShopApp.Entities;
using System.Collections.Generic;

namespace ShopApp.WebUI.Models
{
    public class ProductDetailsModel
    {
        public Product Product{ get; set; }
        public List<Category> Categories{ get; set; }
    }
}
