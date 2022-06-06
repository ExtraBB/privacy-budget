using CsvHelper;
using PrivacyBudgetServer.Models.Database;
using PrivacyBudgetServer.Parsers.Options;
using System.Globalization;

namespace PrivacyBudgetServer.Parsers
{
    internal class TransactionCSVParser : ICSVParser<Transaction>
    {
        private string _accountId;
        private TransactionCSVParserOptions _options;

        public TransactionCSVParser(string accountId, TransactionCSVParserOptions options)
        {
            _accountId = accountId;
            _options = options;
        }

        public List<Transaction> TryParseAll(CsvReader csv)
        {
            List<Transaction> result = new List<Transaction>();

            if (_options.HasHeaderRow)
            {
                csv.Read();
                csv.ReadHeader();
            }

            while (csv.Read())
            {
                bool transactionParsed = TryParseLine(csv, out Transaction? transaction);
                if (transactionParsed && transaction != null)
                {
                    result.Add(transaction);
                }
            }

            return result;
        }

        public bool TryParseLine(CsvReader csv, out Transaction? result)
        {
            bool dateFound = csv.TryGetField<string>(_options.DateColumn, out string dateString);
            bool amountFound = csv.TryGetField<decimal>(_options.AmountColumn, out decimal amount);

            if (!amountFound || !dateFound || !DateTime.TryParseExact(dateString, _options.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                result = null;
                return false;
            }

            csv.TryGetField<string>(_options.CounterPartyColumn, out string counterParty);
            csv.TryGetField<string>(_options.CounterPartyAccountColumn, out string counterPartyAccount);
            csv.TryGetField<string>(_options.DescriptionColumn, out string description);

            result = new Transaction(null, _accountId, date, counterParty, counterPartyAccount, amount, description);
            return true;
        }
    }
}
