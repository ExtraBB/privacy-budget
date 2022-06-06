using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PrivacyBudgetServer.Models.Database
{
    public class Account : IMongoDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal StartingBalance { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Balance { get; set; }
        public AccountType Type { get; set; }

        public Account(
            string? id,
            string name,
            string number,
            decimal startingBalance,
            decimal balance,
            AccountType type = AccountType.None)
        {
            Id = id;
            Name = name;
            Number = number;
            Type = type;
            StartingBalance = startingBalance;
            Balance = balance;
        }
    }
}
