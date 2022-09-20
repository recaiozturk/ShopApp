using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public class EfCoreGenericRepository<T, TContext> : IRepository<T>
        where T : class
        where TContext : DbContext, new()
        //kısıtlamaları ekledddik
    {
        public void Create(T entity)
        {
            using (var context = new TContext())
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();
            }
        }

        public async Task<T> CreateAsync(T entity)
        {
            using (var context = new TContext())
            {
                await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }

        public void Delete(T entity)
        {
            using (var context = new TContext())
            {
                context.Set<T>().Remove(entity);
                context.SaveChanges();
            }
        }

        public async Task<List<T>>GetAll(Expression<Func<T, bool>> filter=null)  //filter = null--> varsayıla ndedğer null olsun
        {
            using (var context = new TContext())
            {
                return  filter == null
                    ? await context.Set<T>().ToListAsync()
                    : await context.Set<T>().Where(filter).ToListAsync();
            }
        }

        public async Task<T> GetById(int id)
        {
            using (var context = new TContext())
            {
                return await context.Set<T>().FindAsync(id);
            }
        }

        public T GetOne(Expression<Func<T, bool>> filter)
        {
            using (var context = new TContext())
            {
                return context.Set<T>().Where(filter).SingleOrDefault();
            }
        }

        public  virtual void  Update(T entity)
        {
            using (var context = new TContext())
            {
                context.Entry(entity).State = EntityState.Modified;  // ilişkili datalarda çalışmaz(Ör:Cart daki CartItems ilişkili listesi)
                context.SaveChanges(true);
            }
        }

        public  virtual async Task  UpdateAsync(T entity)
        {
            using (var context = new TContext())
            {
                context.Entry(entity).State = EntityState.Modified;  // ilişkili datalarda çalışmaz(Ör:Cart daki CartItems ilişkili listesi)
                await context.SaveChangesAsync(true);
            }
        }
    }
}
