﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.Abstract
{
    public interface IRepository<T>
    {
        Task<T> GetById(int id);
        T GetOne(Expression<Func<T, bool>> filter); //parametre olarak linq sorgusu alır
        Task<List<T>> GetAll(Expression<Func<T, bool>> filter=null); //Iqueryble --> tekrar sorgulanabilir

        void Create(T entity);
        Task<T> CreateAsync(T entity);
        void Update(T entity);
        Task UpdateAsync(T entity);
        void Delete(T entity);
    }
}
