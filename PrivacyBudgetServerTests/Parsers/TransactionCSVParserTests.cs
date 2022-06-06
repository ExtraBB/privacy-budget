using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PrivacyBudgetServer.Parsers;
using PrivacyBudgetServer.Parsers.Options;
using System.Text;
using System.IO;
using CsvHelper;
using System.Globalization;
using PrivacyBudgetServer.Models.Database;
using System.Collections.Generic;

namespace PrivacyBudgetServerTests.Parsers
{
    [TestClass]
    public class TransactionCSVParserTests
    {
        private static MemoryStream CreateCSVFile(string[]? headerRow, string[][] values, char separator = ',')
        {
            StringBuilder sb = new StringBuilder();
            if(headerRow != null)
            {
                for(int i = 0; i < headerRow.Length; i++)
                {
                    sb.Append(headerRow[i]);
                    if(i < headerRow.Length - 1)
                    {
                        sb.Append(separator);
                    }
                }
                sb.Append('\n');
            }

            foreach(string[] row in values)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    sb.Append(row[i]);
                    if (i < row.Length - 1)
                    {
                        sb.Append(separator);
                    }
                }
                sb.Append('\n');
            }

            return new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
        }

        [TestMethod]
        public void TransactionCSVParser_constructor()
        {
            // Any argument null should throw
            Assert.ThrowsException<ArgumentNullException>(() => new TransactionCSVParser(null, null), "accountId");
            Assert.ThrowsException<ArgumentNullException>(() => new TransactionCSVParser("test_id", null), "options");
            Assert.ThrowsException<ArgumentNullException>(() => new TransactionCSVParser(null, new TransactionCSVParserOptions()), "accountId");

            // Should not throw on valid arguments
            var _ = new TransactionCSVParser("test", new TransactionCSVParserOptions());
        }

        [TestMethod]
        public void TransactionCSVParser_TryParseLine_NullArgument()
        {
            var parser = new TransactionCSVParser("test", new TransactionCSVParserOptions());
            Assert.ThrowsException<ArgumentNullException>(() => parser.TryParseLine(null, out _), "csv");
        }

        [TestMethod]
        public void TransactionCSVParser_TryParseLine_NoDate()
        {
            string[][] testCases = new string[][]
            {
                new string[] { "no_date", "23.0" },
                new string[] { "", "23.0" },
                new string[] { "32-04-2020", "23.0" },
                new string[] { "04/01/2020", "23.0" },
            };

            TransactionCSVParser parser = new TransactionCSVParser("account", new TransactionCSVParserOptions()
            {
                DateColumn = 0,
                AmountColumn = 1
            });

            foreach (string[] testCase in testCases)
            {
                using (var reader = new StreamReader(CreateCSVFile(null, new string[][] { testCase })))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    Assert.IsFalse(parser.TryParseLine(csv, out Transaction? result));
                    Assert.IsNull(result);
                }
            }
        }

        [TestMethod]
        public void TransactionCSVParser_TryParseLine_NoAmount()
        {
            string[][] testCases = new string[][]
            {
                new string[] { "01-04-2020", "23.0.2" },
                new string[] { "01-04-2020", "text" },
                new string[] { "01-04-2020", "" },
            };

            TransactionCSVParser parser = new TransactionCSVParser("account", new TransactionCSVParserOptions()
            {
                DateColumn = 0,
                AmountColumn = 1
            });

            foreach (string[] testCase in testCases)
            {
                using (var reader = new StreamReader(CreateCSVFile(null, new string[][] { testCase })))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    Assert.IsFalse(parser.TryParseLine(csv, out Transaction? result));
                    Assert.IsNull(result);
                }
            }
        }

        [TestMethod]
        public void TransactionCSVParser_TryParseLine_ValidRows()
        {
            string[][] testCases = new string[][]
            {
                new string[] { "01-04-2020", "23.0", "User A", "User A account", "Description X" },
                new string[] { "01-04-2020", "23.0", "User A", "User A account"  },
                new string[] { "01-04-2020", "23.0", "User A"  },
                new string[] { "01-04-2020", "23.0"  },
            };

            TransactionCSVParser parser = new TransactionCSVParser("account", new TransactionCSVParserOptions()
            {
                DateColumn = 0,
                AmountColumn = 1,
                CounterPartyColumn = 2,
                CounterPartyAccountColumn = 3,
                DescriptionColumn = 4
            });

            foreach (string[] testCase in testCases)
            {
                using (var reader = new StreamReader(CreateCSVFile(null, new string[][] { testCase })))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    Assert.IsTrue(parser.TryParseLine(csv, out Transaction? result));
                    Assert.IsNotNull(result);
                    Assert.AreEqual(new DateTime(2020, 4, 1), result.Date);
                    Assert.AreEqual(23.0M, result.Amount);
                    Assert.AreEqual(testCase.Length >= 3 ? "User A" : null, result.CounterParty);
                    Assert.AreEqual(testCase.Length >= 4 ? "User A account" : null, result.CounterPartyAccount);
                    Assert.AreEqual(testCase.Length >= 5 ? "Description X" : null, result.Description);
                }
            }
        }

        [TestMethod]
        public void TransactionCSVParser_TryParseAll_NullArgument()
        {
            var parser = new TransactionCSVParser("test", new TransactionCSVParserOptions());
            Assert.ThrowsException<ArgumentNullException>(() => parser.TryParseAll(null), "csv");
        }

        [TestMethod]
        public void TransactionCSVParser_TryParseAll_NoHeaders()
        {
            TransactionCSVParser parser = new TransactionCSVParser("account", new TransactionCSVParserOptions()
            {
                DateColumn = 0,
                AmountColumn = 1,
                CounterPartyColumn = 2,
                CounterPartyAccountColumn = 3,
                DescriptionColumn = 4
            });

            string[][] values = new string[][]
            {
                new string[] { "01-04-2020", "23.0", "User 1", "User account 1", "Description 1" },
                new string[] { "02-04-2020", "24.0", "User 2", "User account 2", "Description 2"  },
                new string[] { "03-04-2020", "25.0", "User 3", "User account 3", "Description 3"  },
                new string[] { "04-04-2020", "26.0", "User 4", "User account 4", "Description 4"  },
            };

            using (var reader = new StreamReader(CreateCSVFile(null, values)))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                List<Transaction> transactions = parser.TryParseAll(csv);
                Assert.AreEqual(4, transactions.Count);
                for(int i = 0; i < transactions.Count; i++)
                {
                    Assert.AreEqual(new DateTime(2020, 4, i + 1), transactions[i].Date);
                    Assert.AreEqual(23.0M + i, transactions[i].Amount);
                    Assert.AreEqual("User " + (i + 1).ToString(), transactions[i].CounterParty);
                    Assert.AreEqual("User account " + (i + 1).ToString(), transactions[i].CounterPartyAccount);
                    Assert.AreEqual("Description " + (i + 1).ToString(), transactions[i].Description);
                }
            }
        }

        [TestMethod]
        public void TransactionCSVParser_TryParseAll_WithHeaders()
        {
            TransactionCSVParser parser = new TransactionCSVParser("account", new TransactionCSVParserOptions()
            {
                HasHeaderRow = true,
                DateColumn = 0,
                AmountColumn = 1,
                CounterPartyColumn = 2,
                CounterPartyAccountColumn = 3,
                DescriptionColumn = 4
            });

            string[] headers = new string[] { "date", "amount", "counter party", "counter party account", "description" };
            string[][] values = new string[][]
            {
                new string[] { "01-04-2020", "23.0", "User 1", "User account 1", "Description 1" },
                new string[] { "02-04-2020", "24.0", "User 2", "User account 2", "Description 2"  },
                new string[] { "03-04-2020", "25.0", "User 3", "User account 3", "Description 3"  },
                new string[] { "04-04-2020", "26.0", "User 4", "User account 4", "Description 4"  },
            };

            using (var reader = new StreamReader(CreateCSVFile(headers, values)))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                List<Transaction> transactions = parser.TryParseAll(csv);
                Assert.AreEqual(4, transactions.Count);
                for (int i = 0; i < transactions.Count; i++)
                {
                    Assert.AreEqual(new DateTime(2020, 4, i + 1), transactions[i].Date);
                    Assert.AreEqual(23.0M + i, transactions[i].Amount);
                    Assert.AreEqual("User " + (i + 1).ToString(), transactions[i].CounterParty);
                    Assert.AreEqual("User account " + (i + 1).ToString(), transactions[i].CounterPartyAccount);
                    Assert.AreEqual("Description " + (i + 1).ToString(), transactions[i].Description);
                }
            }
        }

        [TestMethod]
        public void TransactionCSVParser_TryParseAll_Empty()
        {
            TransactionCSVParser parser = new TransactionCSVParser("account", new TransactionCSVParserOptions()
            {
                DateColumn = 0,
                AmountColumn = 1,
                CounterPartyColumn = 2,
                CounterPartyAccountColumn = 3,
                DescriptionColumn = 4
            });

            string[][] values = new string[][]
            {
            };

            using (var reader = new StreamReader(CreateCSVFile(null, values)))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                List<Transaction> transactions = parser.TryParseAll(csv);
                Assert.AreEqual(0, transactions.Count);
            }
        }

        [TestMethod]
        public void TransactionCSVParser_TryParseAll_InvalidRows()
        {
            TransactionCSVParser parser = new TransactionCSVParser("account", new TransactionCSVParserOptions()
            {
                DateColumn = 0,
                AmountColumn = 1,
                CounterPartyColumn = 2,
                CounterPartyAccountColumn = 3,
                DescriptionColumn = 4
            });

            string[][] values = new string[][]
            {
                new string[] { "01-04-2020", "23.0", "User 1", "User account 1", "Description 1" },
                new string[] { "invalid", "invalid amount", "User invalid", "User account invalid", "Description invalid"  },
                new string[] { "02-04-2020", "24.0", "User 2", "User account 2", "Description 2"  },
                new string[] { "03-04-2020", "25.0", "User 3", "User account 3", "Description 3"  },
            };

            using (var reader = new StreamReader(CreateCSVFile(null, values)))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                List<Transaction> transactions = parser.TryParseAll(csv);
                Assert.AreEqual(3, transactions.Count);
                for (int i = 0; i < transactions.Count; i++)
                {
                    Assert.AreEqual(new DateTime(2020, 4, i + 1), transactions[i].Date);
                    Assert.AreEqual(23.0M + i, transactions[i].Amount);
                    Assert.AreEqual("User " + (i + 1).ToString(), transactions[i].CounterParty);
                    Assert.AreEqual("User account " + (i + 1).ToString(), transactions[i].CounterPartyAccount);
                    Assert.AreEqual("Description " + (i + 1).ToString(), transactions[i].Description);
                }
            }
        }
    }
}
