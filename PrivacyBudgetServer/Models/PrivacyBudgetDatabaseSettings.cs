namespace PrivacyBudgetServer.Models
{
    public class PrivacyBudgetDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string TransactionsCollectionName { get; set; } = null!;
    }
}
