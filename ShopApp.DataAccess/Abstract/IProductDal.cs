using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.Abstract
{
    public  interface IProductDal:IRepository<Product>
    {
        IEnumerable<Product> GetPopolerProduct();

        Product GetProductDetails(int id);
    }
}
