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

        List<Product> GetProductsByCategorry(string category,int page, int pageSize);

        Product GetProductDetails(int id);
        int GetCountByCategory(string category);
        Product GetByIdWithCategories(int id);
        void Update(Product entity, int[] categoryIds);
        Task UpdateAsync(Product entity, int[] categoryIds);
    }
}
