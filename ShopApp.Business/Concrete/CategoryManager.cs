using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Business.Concrete
{
    public class CategoryManager : IcategoryServicee
    {
        private ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public void Create(Category category)
        {
             _categoryDal.Create(category);
        }

    

        public void Delete(Category category)
        {
            _categoryDal.Delete(category);
        }

        public void DeleteFromCategory(int categoryId, int productId)
        {
            _categoryDal.DeleteFromCategory(categoryId, productId);
        }

        public async Task<List<Category>> GetAll()
        {
            return await _categoryDal.GetAll();
        }

        public async Task<Category> GetById(int id)
        {
            return await _categoryDal.GetById(id);
        }

        public Category GetByIdWithProducts(int id)
        {
            return _categoryDal.GetByIdWithProducts(id);
        }

        public void Update(Category category)
        {
            _categoryDal.Update(category);
        }

        
    }
}
