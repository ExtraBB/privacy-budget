namespace PrivacyBudgetServer.Parsers
{
    public class CSVParserOptions
    {
        public bool HasHeaderRow { get; set; }
        public string DateFormat { get; set; } = "dd-MM-yyyy";
    }
}
