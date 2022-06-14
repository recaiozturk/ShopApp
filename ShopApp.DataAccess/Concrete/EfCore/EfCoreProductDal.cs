using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public class EfCoreProductDal : EfCoreGenericRepository<Product, ShopContext>, IProductDal
    {
        public IEnumerable<Product> GetPopolerProduct()
        {
            throw new NotImplementedException();
        }

        public Product GetProductDetails(int id)
        {
            //despose işlemi
            using (var context= new ShopContext())
            {
                return context.Products
                    .Where(p => p.Id == id)
                    .Include(p => p.ProductCategories)
                    .ThenInclude(p => p.Category)
                    .FirstOrDefault();
            }
        }
    }
}
