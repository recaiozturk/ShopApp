using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Business.Abstract
{
    public interface IProductService
    {
        Task<Product> GetById(int id); 
        Task<List<Product>> GetAll();
        List<Product> GetPopulerProducts();
        List<Product> GetProductsByCategorry(string category, int page, int pageSize);

        int GetCountByCategory(string category);

        Product GetProductDetails(int id);

        void Create(Product product);
        Task<Product> CreateAsync(Product product);
        void Update(Product product);
        Task UpdateAsync(Product product);
        void Delete(Product Product);
        Task DeleteAsync(Product Product);
        Product GetByIdWithCategories(int id);

        //Updadte overloaded versiyon(Aşırı yüklenmiş hali)
        void Update(Product entity, int[] categoryIds);
        Task UpdateAsync(Product entity, int[] categoryIds);
    }
}
