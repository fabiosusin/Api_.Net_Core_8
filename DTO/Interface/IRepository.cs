using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DTO.Interface
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity Insert(IBaseData obj);
        void Remove(IBaseData obj);
        void RemoveById(string id);
        IEnumerable<TEntity> FindAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity FindById(string id);
        TEntity Update(IBaseData obj);
        int SaveChanges();
    }
}
