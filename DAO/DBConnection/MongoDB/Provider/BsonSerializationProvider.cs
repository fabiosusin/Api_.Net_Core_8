using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace DAO.DBConnection.MongoDB.Provider
{
    public class BsonSerializationProvider : IBsonSerializationProvider
    {
        public IBsonSerializer GetSerializer(Type type)
        {
            if (type == typeof(decimal))
                return new DecimalSerializer(BsonType.Decimal128);
            if (type == typeof(decimal?))
                return new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128));

            return null;
        }
    }
}
