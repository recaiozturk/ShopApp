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

        List<Product> GetProductsByCategorry(string category,int page);

        Product GetProductDetails(int id);
    }
}
