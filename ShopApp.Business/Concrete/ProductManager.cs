using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EfCore;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Business.Concrete
{
    public class ProductManager : IProductService 
    {
        //depencency injection
        private IProductDal _productDal;
        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public void Create(Product product)
        {
            _productDal.Create(product);
        }

        public void Delete(Product product)
        {
            _productDal.Delete(product);
        }

        public List<Product> GetAll()
        {
            return _productDal.GetAll().ToList();
        }

        public Product GetById(int id)
        {
            return _productDal.GetById(id);
        }

        public void Update(Product product)
        {
          _productDal.Update(product);
        }
    }
}
