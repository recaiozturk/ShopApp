using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Business.Abstract
{
    public interface IcategoryServicee
    {
        Task<Category> GetById(int id);

        //Kategori ile ilişkili ürünleri getir
        Category GetByIdWithProducts(int id);
        Task<List<Category>> GetAll();
        void Create(Category category);
        void Update(Category category);
        void Delete(Category category);
        void DeleteFromCategory(int categoryId, int productId);
    }
}
