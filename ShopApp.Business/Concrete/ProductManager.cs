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

        public async Task<Product> CreateAsync(Product product)
        {
            await _productDal.CreateAsync(product);
            return product;
        }



        public void Delete(Product product)
        {
            _productDal.Delete(product);
        }

        public async Task<List<Product>> GetAll()
        {
            return await _productDal.GetAll();
        }

        public async Task<Product> GetById(int id)
        {
            return  await _productDal.GetById(id);
        }

        //products lar ile birlikte categorileri de getir
        public Product GetByIdWithCategories(int id)
        {
            return _productDal.GetByIdWithCategories(id);
        }

        //sayfadaki categoriye göre ürün sayısını verir
        public int GetCountByCategory(string category)
        {
            return _productDal.GetCountByCategory(category);
        }

        public async Task<List<Product>> GetPopulerProducts()
        {
            return await  _productDal.GetAll();
        }

        public Product GetProductDetails(int id)
        {
            return _productDal.GetProductDetails(id);
        }

        public List<Product> GetProductsByCategorry(string category, int page, int pageSize)
        {
            return _productDal.GetProductsByCategorry(category,page,pageSize);
        }

        public void Update(Product product)
        {
          _productDal.Update(product);
        }

        public async Task UpdateAsync(Product product)
        {
            await _productDal.UpdateAsync(product);
        }

        public void Update(Product entity, int[] categoryIds)
        {
            _productDal.Update(entity, categoryIds);
        }


        public async Task UpdateAsync(Product entity, int[] categoryIds)
        {
            await _productDal.UpdateAsync(entity, categoryIds);
        }



        List<Product> IProductService.GetPopulerProducts()
        {
            throw new NotImplementedException();
        }


        public string ErrorMessages { get; set; }

        //Test
        //public bool Validate(Product entity)
        //{
        //    var isValid = true;

        //    if (string.IsNullOrEmpty(entity.Name))
        //    {
        //        ErrorMessages += "Ürün ismi Girmelisiniz";
        //    }

        //    if (string.IsNullOrEmpty(entity.Name))
        //    {
        //        ErrorMessages += "Ürün ismi Girmelisiniz";
        //    }

        //    //daha Fazla Eklenebilir

        //    return isValid;
        //}
    }
}
