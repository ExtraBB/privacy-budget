using CsvHelper;

namespace PrivacyBudgetServer.Parsers
{
    public interface ICSVParser<T>
    {
        bool TryParseLine(CsvReader csv, out T? result);
        List<T> TryParseAll(CsvReader csv);
    }
}
