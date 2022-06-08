using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.Abstract
{
    public  interface ICategoryDal
    {

        Category GetById(int id);
        Category GetOne(Expression<Func<Category, bool>> filter); //parametre olarak linq sorgusu alır
        IQueryable<Category> GetAll(Expression<Func<Category, bool>> filter); //Iqueryble --> tekrar sorgulanabilir

        void Create(Category product);
        void Update(Category product);
        void Delete(Category product);
    }

}
