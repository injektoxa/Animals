using Animals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Animals.Repository
{
    public interface IRepository<T> 
    {
        IQueryable<T> FindAll(Func<T, bool> exp);
        

        IQueryable<T> FindAll();
        
        T Find(Guid id);

        void Add(T entity);

        void SaveAll();
        
        void Delete(T entity);

        void Update(T entity);
    }
}