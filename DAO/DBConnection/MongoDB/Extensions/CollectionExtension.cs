using DTO.General.Base.Database;
using DTO.Interface;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;

namespace DAO.DBConnection.MongoDB.Extensions
{
    public static class CollectionExtension
    {

        public static void UpdateById(this MongoCollection collection, IBaseData value)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            collection.Update(Query<BaseData>.EQ(x => x.Id, value.Id), Update<IBaseData>.Replace(value));
        }

        public static void UpdateById<T>(this MongoCollection<T> collection, string id, UpdateBuilder<T> updateBuilder)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            if (updateBuilder == null)
                throw new ArgumentNullException(nameof(updateBuilder));

            collection.Update(Query<BaseData>.EQ(x => x.Id.ToString(), id), updateBuilder);
        }

        public static void RemoveById(this MongoCollection collection, string id)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            collection.Remove(Query<BaseData>.EQ(x => x.Id, id));
        }

        public static string Upsert<TEntity>(this MongoCollection<TEntity> collection, BaseData value) where TEntity : BaseData
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var currentData = collection.FindById(value.Id.ToString());
            if (currentData == null)
                collection.Add(value);
            else
                collection.UpdateById(value);
            return value.Id.ToString();
        }

        public static string Add(this MongoCollection collection, IBaseData value)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (!ObjectId.TryParse(value.Id, out _))
                value.Id = ObjectId.GenerateNewId().ToString();
            collection.Insert(value);
            return value.Id.ToString();
        }

        public static TEntity FindById<TEntity>(this MongoCollection<TEntity> collection, string id)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            return ObjectId.TryParse(id, out var _) ? collection.FindOneByIdAs<TEntity>(new ObjectId(id)) : default;
        }
    }
}
