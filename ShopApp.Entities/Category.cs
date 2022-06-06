using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }

        //çoka çok ilişki olmasayddı bu şekilded yapacaktık
        //public List<Product> Products { get; set; }
    }
}
