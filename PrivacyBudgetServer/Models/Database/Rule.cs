using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PrivacyBudgetServer.Models.Database
{
    public class Rule : IMongoDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AccountId { get; set; }
        public RuleSegment Segment { get; set; }
        public TransactionCategory TransactionCategory { get; set; } = TransactionCategory.Undetermined;

        public Rule(string accountId, RuleSegment segment)
        {
            AccountId = accountId;
            Segment = segment;
        }
    }
}
