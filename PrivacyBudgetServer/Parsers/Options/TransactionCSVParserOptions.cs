namespace PrivacyBudgetServer.Parsers.Options
{
    internal class TransactionCSVParserOptions : CSVParserOptions
    {
        public int DateColumn { get; set; } = -1;

        public int CounterPartyColumn { get; set; } = -1;

        public int CounterPartyAccountColumn { get; set; } = -1;

        public int AmountColumn { get; set; } = -1;

        public int DescriptionColumn { get; set; } = -1;
    }
}
