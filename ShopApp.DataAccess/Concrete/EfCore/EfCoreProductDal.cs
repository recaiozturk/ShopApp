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
    public class EfCoreProductDal : EfCoreGenericRepository<Product, ShopContext>, IProductDal
    {
        public Product GetByIdWithCategories(int id)
        {
            using (var context = new ShopContext())
            {
                return context.Products
                    .Where(p => p.Id == id)
                    .Include(p=>p.ProductCategories)
                    .ThenInclude(p=>p.Category)
                    .FirstOrDefault();
            }
        }

        public int GetCountByCategory(string category)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.AsQueryable();

                //category stringi null değil ise
                if (!string.IsNullOrEmpty(category))
                {
                    //Caategory ye ulaşmak için daha önce CategoryProduct a ulaşıyoruz,sql dedki join işlemi gibi
                    products = products
                                .Include(i => i.ProductCategories)
                                .ThenInclude(i => i.Category)
                                .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
                }

                return products.Count();
            }
        }

        public IEnumerable<Product> GetPopolerProduct()
        {
            throw new NotImplementedException();
        }

        public Product GetProductDetails(int id)
        {
            //despose işlemi
            using (var context= new ShopContext())
            {
                return context.Products
                    .Where(p => p.Id == id)
                    .Include(p => p.ProductCategories)
                    .ThenInclude(p => p.Category)
                    .FirstOrDefault();
            }
        }

        public List<Product> GetProductsByCategorry(string category, int page,int pageSize)
        {
            using(var context= new ShopContext())
            {
                var products = context.Products.AsQueryable();

                //category stringi null değil ise
                if (!string.IsNullOrEmpty(category))
                {
                    //Caategory ye ulaşmak için daha önce CategoryProduct a ulaşıyoruz,sql dedki join işlemi gibi
                    products = products
                                .Include(i => i.ProductCategories)
                                .ThenInclude(i => i.Category)
                                .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
                }

                return products.Skip((page-1)*pageSize).Take(pageSize).ToList();
            }
        }

        //Update Overloaded
        public void Update(Product entity, int[] categoryIds)
        {
            using (var context = new ShopContext())
            {
                var product = context.Products
                    .Include(i => i.ProductCategories)
                    .FirstOrDefault(p => p.Id == entity.Id);

                if(product!= null)
                {
                    product.Name=entity.Name;
                    product.Description=entity.Description;
                    product.ImageUrl=entity.ImageUrl;
                    product.Price=entity.Price;

                    product.ProductCategories = categoryIds.Select(catId => new ProductCategory()
                    {
                        CategoryId=catId,
                        ProductId=entity.Id
                    }).ToList();

                    context.SaveChanges();
                }
            }
        }

        //UpdateAsync Overloaded
        public async Task  UpdateAsync(Product entity, int[] categoryIds)
        {
            using (var context = new ShopContext())
            {
                var product = context.Products
                    .Include(i => i.ProductCategories)
                    .FirstOrDefault(p => p.Id == entity.Id);

                if (product != null)
                {
                    product.Name = entity.Name;
                    product.Description = entity.Description;
                    product.ImageUrl = entity.ImageUrl;
                    product.Price = entity.Price;

                    product.ProductCategories = categoryIds.Select(catId => new ProductCategory()
                    {
                        CategoryId = catId,
                        ProductId = entity.Id
                    }).ToList();

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
