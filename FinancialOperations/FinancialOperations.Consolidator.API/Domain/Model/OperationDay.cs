using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FinancialOperations.Consolidator.API.Domain.Model
{
    public class OperationDay
    {

        [BsonId]
        public ObjectId Id { get; set; }
        public DateTime Day { get; set; }
        public decimal Total { get; set; }
    }
}
