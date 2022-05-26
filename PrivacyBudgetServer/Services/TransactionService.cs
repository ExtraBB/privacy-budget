using PrivacyBudgetServer.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PrivacyBudgetServer.Services
{
    public class TransactionService
    {
        private readonly IMongoCollection<Transaction> _transactionsCollection;

        public TransactionService(IOptions<PrivacyBudgetDatabaseSettings> privacyBudgetDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                privacyBudgetDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                privacyBudgetDatabaseSettings.Value.DatabaseName);

            _transactionsCollection = mongoDatabase.GetCollection<Transaction>(
                privacyBudgetDatabaseSettings.Value.TransactionsCollectionName);
        }

        public async Task<List<Transaction>> GetAsync() =>
            await _transactionsCollection.Find(_ => true).ToListAsync();

        public async Task<Transaction?> GetAsync(string id) =>
            await _transactionsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Transaction newTransaction) =>
            await _transactionsCollection.InsertOneAsync(newTransaction);

        public async Task UpdateAsync(string id, Transaction updatedTransaction) =>
            await _transactionsCollection.ReplaceOneAsync(x => x.Id == id, updatedTransaction);

        public async Task RemoveAsync(string id) =>
            await _transactionsCollection.DeleteOneAsync(x => x.Id == id);
    }
}
