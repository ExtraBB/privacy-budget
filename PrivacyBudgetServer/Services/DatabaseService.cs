using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PrivacyBudgetServer.Models;

namespace PrivacyBudgetServer.Services
{
    public class DatabaseService
    {
        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }
        public IMongoCollection<Transaction> TransactionCollection { get; }

        public DatabaseService(IOptions<PrivacyBudgetDatabaseSettings> privacyBudgetDatabaseSettings)
        {
            Client = new MongoClient(privacyBudgetDatabaseSettings.Value.ConnectionString);
            Database = Client.GetDatabase(privacyBudgetDatabaseSettings.Value.DatabaseName);
            TransactionCollection = Database.GetCollection<Transaction>(privacyBudgetDatabaseSettings.Value.TransactionsCollectionName);
        }
    }
}
