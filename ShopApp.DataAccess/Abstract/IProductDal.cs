using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.Abstract
{
    public  interface IProductDal
    {
        Product GetById(int id);
        Product GetOne(Expression<Func<Product, bool>> filter); //parametre olarak linq sorgusu alır
        IQueryable<Product> GetAll(Expression<Func<Product, bool>> filter); //Iqueryble --> tekrar sorgulanabilir

        void Create(Product product);
        void Update(Product product);
        void Delete(Product product);
    }
}
