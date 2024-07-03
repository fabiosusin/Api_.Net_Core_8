using DTO.Interface;

namespace DAO.DBConnection.MongoDB.Settings
{
    public class MongoDBSettings : IMongoDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IMongoDBSettings : IDBSettings { }
}
