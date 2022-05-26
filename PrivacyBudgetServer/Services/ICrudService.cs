namespace PrivacyBudgetServer.Services
{
    public interface ICRUDService<T>
    {
        Task<List<T>> GetAsync();
        Task<T?> GetAsync(string id);
        Task CreateAsync(T newT);
        Task UpdateAsync(string id, T updatedTransaction);
        Task RemoveAsync(string id);
    }
}
