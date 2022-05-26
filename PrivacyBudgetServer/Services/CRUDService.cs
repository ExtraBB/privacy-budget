using PrivacyBudgetServer.Models;
using MongoDB.Driver;

namespace PrivacyBudgetServer.Services
{
    public class CRUDService<T> : ICRUDService<T> where T : IMongoDocument
    {
        private readonly IMongoCollection<T> _collection;

        public CRUDService(IMongoCollection<T>? collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            _collection = collection;
        }

        public async Task<List<T>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<T?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(T newTransaction) =>
            await _collection.InsertOneAsync(newTransaction);

        public async Task UpdateAsync(string id, T updatedTransaction) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updatedTransaction);

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}
