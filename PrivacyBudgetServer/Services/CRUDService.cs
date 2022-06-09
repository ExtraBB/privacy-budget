using PrivacyBudgetServer.Models.Database;
using MongoDB.Driver;
using System.Linq.Expressions;

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

        public async Task<List<T>> GetAsync(Expression<Func<T,bool>> filter) =>
            await _collection.Find(filter).ToListAsync();

        public async Task CreateAsync(T newTransaction) =>
            await _collection.InsertOneAsync(newTransaction);

        public async Task CreateManyAsync(IEnumerable<T> newTransactions) =>
            await _collection.InsertManyAsync(newTransactions);

        public async Task UpdateAsync(string id, T updatedTransaction) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updatedTransaction);

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);

        public async Task RemoveWhereAsync(Expression<Func<T, bool>> filter) =>
            await _collection.DeleteManyAsync(filter);
    }
}
