
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FinancialOperations.Consolidator.API.Domain.Model
{
    public class ProcessedOperation
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public DateTime Day { get; set; }
        public bool IsCredit { get; set; }
        public decimal Value { get; set; }
    }
}
