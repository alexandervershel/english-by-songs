using Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IRepository<T> where T : class, IModel, new()
    {
        void Add(T item);
        void Dispose();
        void Dispose(bool disposing);
        T Get(Expression<Func<T, bool>> predicate);
        T Get(int id);
        List<T> GetAll();
        List<T> GetAll(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(int id);
        void Remove(T item);
        void Save();
        Task SaveAsync();
        void Update(T item);
    }
}