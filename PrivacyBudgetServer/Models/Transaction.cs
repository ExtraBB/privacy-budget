using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PrivacyBudgetServer.Models
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public DateTime Date { get; set; }
    }
}
