using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DTO.Interface
{
    public interface IBaseData
    {
        [BsonId]
        string Id { get; set; }
    }
}
