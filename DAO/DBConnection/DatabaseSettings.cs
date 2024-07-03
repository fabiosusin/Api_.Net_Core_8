using DAO.DBConnection.MongoDB.Settings;

namespace DAO.DBConnection
{
    public class DatabaseSettings : IXDataDatabaseSettings
    {
        public MongoDBSettings MongoDBSettings { get; set; }
    }

    public interface IXDataDatabaseSettings
    {
        MongoDBSettings MongoDBSettings { get; set; }
    }
}
