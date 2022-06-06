namespace PrivacyBudgetServer.Parsers
{
    internal class CSVParserOptions
    {
        public bool HasHeaderRow { get; set; }
        public string DateFormat { get; set; } = "dd-MM-yyyy";
    }
}
