using MongoDB.Driver;
using System.Linq.Expressions;

namespace PrivacyBudgetServer.Services
{
    public interface ICRUDService<T>
    {
        Task<List<T>> GetAsync();
        Task<T?> GetAsync(string id);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> filter);
        Task CreateAsync(T newT);
        Task CreateManyAsync(IEnumerable<T> newTs);
        Task UpdateAsync(string id, T updatedTransaction);
        Task RemoveAsync(string id);
        Task RemoveWhereAsync(Expression<Func<T, bool>> filter);
    }
}
