using Animals.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace Animals.Repository
{
    public class SQLRepository<T> : IRepository<T> where T : class
    {
        private AnimalsEntities _context;

        public SQLRepository(AnimalsEntities context)
        {
            _context = context;
        }

        public IQueryable<T> FindAll(Func<T, bool> exp)
        {
            return _context.Set<T>().Where(exp).AsQueryable();
        }

        public IQueryable<T> FindAll()
        {
            return _context.Set<T>();
        }
        
        public T Find(Guid id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Entity cannot be null");
            }

            _context.Set<T>().Remove(entity);
        }

        public void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Entity cannot be null");
            }

            // New entity
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);

            _context.Entry(entity).State = EntityState.Modified;
        }

        public void SaveAll()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}