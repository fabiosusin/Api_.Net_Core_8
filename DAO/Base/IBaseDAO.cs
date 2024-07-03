using DTO.General.DAO.Output;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAO.Base
{
    public interface IBaseDAO<TEntity>
    {
        DAOActionResultOutput Upsert(TEntity obj);
        DAOActionResultOutput Insert(TEntity obj);
        DAOActionResultOutput Remove(TEntity obj);
        DAOActionResultOutput Update(TEntity obj);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity FindOne();
        TEntity FindOne(Expression<Func<TEntity, bool>> predicate);
        TEntity FindById(string id);
        IEnumerable<TEntity> FindAll();
        DAOActionResultOutput RemoveById(string id);
    }
}
