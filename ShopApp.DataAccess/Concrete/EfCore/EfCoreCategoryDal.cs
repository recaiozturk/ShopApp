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
    public class EfCoreCategoryDal : EfCoreGenericRepository<Category, ShopContext>, ICategoryDal
    {
        public void DeleteFromCategory(int categoryId, int productId)
        {
            using (var context = new ShopContext())
            {
                var cmd = @"delete from ProductCategory where ProductId=@p0 and CategoryId=@p1";
                context.Database.ExecuteSqlRaw(cmd, productId, categoryId);

                
            }
        }

        public Category GetByIdWithProducts(int id)
        {
            using(var context = new ShopContext())
            {
                return context.Categories
                    .Where(c => c.Id == id)
                    .Include(c => c.ProductCategories)
                    .ThenInclude(c => c.Product)
                    .FirstOrDefault();
            }
        }
    }
}
