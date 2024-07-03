using DAO.DBConnection.MongoDB.Extensions;
using DAO.DBConnection.MongoDB.Provider;
using DAO.DBConnection.MongoDB.Settings;
using DTO.General.Base.Database;
using DTO.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAO.DBConnection
{
    internal class RepositoryMongo<TEntity> : IRepository<TEntity> where TEntity : BaseData
    {
        public MongoCollection<TEntity> Collection { get; }

        internal RepositoryMongo(IMongoDBSettings settings) => Collection = new DbAccess(settings).MongoDatabase.GetCollection<TEntity>();

        public virtual TEntity Insert(IBaseData obj)
        {
            Collection.Add(obj);
            return (TEntity)obj;
        }

        public void Remove(IBaseData obj) => RemoveById(obj.Id);

        public virtual TEntity Update(IBaseData obj)
        {
            Collection.UpdateById(obj);
            return (TEntity)obj;
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) => Collection.Find(Query<TEntity>.Where(predicate));

        public virtual TEntity FindOne() => Collection.FindOne();

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> predicate) => Collection.FindOne(Query<TEntity>.Where(predicate));

        public virtual TEntity FindById(string id) => Collection.FindById(id?.ToString());

        public virtual IEnumerable<TEntity> FindAll() => Collection.FindAll();

        public virtual void RemoveById(string id) => Collection.RemoveById(id);

        public int SaveChanges() => 1;

        public void Dispose() { }
    }
}
