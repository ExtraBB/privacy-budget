using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PrivacyBudgetServer.Models.Database;

namespace PrivacyBudgetServer.Services
{
    public class DatabaseService
    {
        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }
        public IMongoCollection<Transaction> TransactionCollection { get; }
        public IMongoCollection<Account> AccountCollection { get; }
        public IMongoCollection<Rule> RuleCollection { get; }

        public DatabaseService(IOptions<PrivacyBudgetDatabaseSettings> privacyBudgetDatabaseSettings)
        {
            Client = new MongoClient(privacyBudgetDatabaseSettings.Value.ConnectionString);
            Database = Client.GetDatabase(privacyBudgetDatabaseSettings.Value.DatabaseName);
            TransactionCollection = Database.GetCollection<Transaction>(privacyBudgetDatabaseSettings.Value.TransactionsCollectionName);
            AccountCollection = Database.GetCollection<Account>(privacyBudgetDatabaseSettings.Value.AccountsCollectionName);
            RuleCollection = Database.GetCollection<Rule>(privacyBudgetDatabaseSettings.Value.RulesCollectionName);
        }
    }
}
