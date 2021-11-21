using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dal.Repositories
{
    public class Repository<T> where T : class, IModel, new()
    {
        private bool _disposed = false;
        protected readonly EnglishBySongsDbContext _db;
        public Repository(EnglishBySongsDbContext db)
        {
            _db = db;
        }

        public virtual T Get(int id)
        {
            return _db.Set<T>().Find(id);
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await _db.Set<T>().FindAsync(id);
        }
        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return _db.Set<T>().FirstOrDefault(predicate);
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _db.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public virtual List<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public virtual List<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _db.Set<T>().Where(predicate).ToList();
        }

        public virtual async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _db.Set<T>().Where(predicate).ToListAsync();
        }

        public virtual void Add(T item)
        {
            _db.Set<T>().Add(item);
        }

        public virtual void Remove(T item)
        {
            _db.Entry(item).State = EntityState.Deleted;
        }

        public virtual void Update(T item)
        {
            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
            _db.Entry(item).State = EntityState.Detached;
        }

        public virtual void Save()
        {
            _db.SaveChanges();
        }

        public virtual async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
