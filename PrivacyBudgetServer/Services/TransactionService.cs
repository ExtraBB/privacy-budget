using PrivacyBudgetServer.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PrivacyBudgetServer.Services
{
    public class TransactionService : ICRUDService<Transaction>
    {
        private readonly IMongoCollection<Transaction> _transactionsCollection;

        public TransactionService(IOptions<PrivacyBudgetDatabaseSettings> privacyBudgetDatabaseSettings, IMongoDatabase database)
        {
            _transactionsCollection = database.GetCollection<Transaction>(
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
