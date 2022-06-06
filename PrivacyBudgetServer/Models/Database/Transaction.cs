using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PrivacyBudgetServer.Models.Database
{
    public class Transaction : IMongoDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string AccountId { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Date { get; set; }
        public string CounterParty { get; set; }
        public string CounterPartyAccount { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public TransactionCategory Category { get; set; }

        public Transaction(
            string? id,
            string accountId,
            DateTime date, 
            string counterParty, 
            string counterPartyAccount, 
            decimal amount, 
            string description, 
            TransactionCategory category = TransactionCategory.Undetermined)
        {
            Id = id;
            AccountId = accountId;
            Date = date;
            CounterParty = counterParty;
            CounterPartyAccount = counterPartyAccount;
            Amount = amount;
            Description = description?.Trim('"', '\'')?.Trim();
            Category = category;
        }
    }
}
