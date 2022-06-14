namespace PrivacyBudgetServer.Models.Database
{
    public class PrivacyBudgetDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string TransactionsCollectionName { get; set; } = null!;
        public string AccountsCollectionName { get; set; } = null!;
        public string RulesCollectionName { get; set; } = null!;
        public string BudgetsCollectionName { get; set; } = null!;
    }
}
