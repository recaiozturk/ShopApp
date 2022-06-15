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

        public List<Product> GetProductsByCategorry(string category, int page)
        {
            using(var context= new ShopContext())
            {
                var products = context.Products.AsQueryable();

                //category stringi null değil ise
                if (!string.IsNullOrEmpty(category))
                {
                    //Caategory ye ulaşmak için daha önce CategoryProduct a ulaşıyoruz,sql dedki join işlemi gibi
                    products = products
                                .Include(i => i.ProductCategories)
                                .ThenInclude(i => i.Category)
                                .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
                }

                return products.ToList();
            }
        }
    }
}
