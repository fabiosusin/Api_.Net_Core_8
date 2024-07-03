using MongoDB.Driver;
using System;

namespace DAO.DBConnection.MongoDB.Extensions
{
    public static class DatabaseExtension
    {

        public static MongoCollection<TEntity> GetCollection<TEntity>(this MongoDatabase db, string name)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));
            return db.GetCollection<TEntity>(name);
        }

        public static MongoCollection<TEntity> GetCollection<TEntity>(this MongoDatabase db)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));
            return db.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public static IMongoCollection<TEntity> GetCollection<TEntity>(this IMongoDatabase db)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));
            return db.GetCollection<TEntity>(typeof(TEntity).Name);
        }
    }
}
