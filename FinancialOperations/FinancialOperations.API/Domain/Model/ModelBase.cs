using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FinancialOperations.API.Domain.Model
{
    public class ModelBase
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
