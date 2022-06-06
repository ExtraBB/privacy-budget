using CsvHelper;

namespace PrivacyBudgetServer.Parsers
{
    internal interface ICSVParser<T>
    {
        bool TryParseLine(CsvReader csv, out T? result);
        List<T> TryParseAll(CsvReader csv);
    }
}
