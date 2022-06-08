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
        Category GetById(int id);
        List<Category> GetAll();
        void Create(Category category);
        void Update(Category category);
        void Delete(int id);
    }
}
