using DTO.Interface;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DTO.General.Base.Database
{
    public class BaseData : IBaseData
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
