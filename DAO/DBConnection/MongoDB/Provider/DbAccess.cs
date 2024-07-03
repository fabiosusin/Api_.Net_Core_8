using DAO.DBConnection.MongoDB.Settings;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace DAO.DBConnection.MongoDB.Provider
{
    public class DbAccess
    {
        public MongoDatabase MongoDatabase;
        public DbAccess(IMongoDBSettings settings)
        {
            BsonSerializer.RegisterSerializationProvider(new BsonSerializationProvider());
            var client = new MongoClient(settings.ConnectionString);
            ConventionRegistry.Register("IgnoreExtraElements", new ConventionPack() { new IgnoreExtraElementsConvention(true) }, type => true);
            MongoDatabase = client.GetServer().GetDatabase(settings.DatabaseName);
        }
    }
}
