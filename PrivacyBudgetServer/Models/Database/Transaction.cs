using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PrivacyBudgetServer.Models.Database
{
    public class Transaction : IMongoDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AccountId { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string FromAccount { get; set; }
        public string To { get; set; }
        public string ToAccount { get; set; }
        public decimal Amount { get; set; }
        public string? Type { get; set; }
        public string Description { get; set; }
        public TransactionCategory Category { get; set; }

        public Transaction(
            string? id,
            string accountId,
            DateTime date, 
            string from,
            string fromAccount, 
            string to, 
            string toAccount, 
            decimal amount, 
            string? type, 
            string description, 
            TransactionCategory category = TransactionCategory.Undetermined)
        {
            Id = id;
            AccountId = accountId;
            Date = date;
            From = from;
            FromAccount = fromAccount;
            To = to;
            ToAccount = toAccount;
            Amount = amount;
            Type = type;
            Description = description;
            Category = category;
        }
    }
}
