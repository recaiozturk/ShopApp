using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Entities
{
    public class Product
    {
        public int  Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }


        //çoka çok ilişki olmasayddı bu şekilde yapacaktık
        //public int CategoryId { get; set; }
        //public Category Category { get; set; }
    }
}
