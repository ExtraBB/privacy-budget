using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PrivacyBudgetServer.Models;
using PrivacyBudgetServer.Models.Database;
using PrivacyBudgetServer.Services;
using System;
using System.Collections.Generic;

namespace PrivacyBudgetServerTests.Services
{
    [TestClass]
    public class RuleProcessingServiceTests
    {
        [TestMethod]
        public void RuleSatisifed_Exists()
        {
            RuleProcessingService service = new RuleProcessingService(new Mock<ICRUDService<Rule>>().Object);

            Transaction transaction = new Transaction(null, "account_id", DateTime.Now, null, "123456abcdef", 230.0M, "TestDescription");

            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.Exists
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));


            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterParty,
                Operator = RuleOperator.Exists
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));
        }

        [TestMethod]
        public void RuleSatisifed_NotExists()
        {
            RuleProcessingService service = new RuleProcessingService(new Mock<ICRUDService<Rule>>().Object);

            Transaction transaction = new Transaction(null, "account_id", DateTime.Now, null, "123456abcdef", 230.0M, "TestDescription");

            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.NotExists
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));


            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterParty,
                Operator = RuleOperator.NotExists
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));
        }

        [TestMethod]
        public void RuleSatisifed_Equals()
        {
            RuleProcessingService service = new RuleProcessingService(new Mock<ICRUDService<Rule>>().Object);

            Transaction transaction = new Transaction(null, "account_id", DateTime.Now, null, "123456abcdef", 230.0M, "TestDescription");

            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.Equals,
                Parameter = "123456abcdef"
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));


            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.Equals,
                Parameter = "123456abcdefg"
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.Equals
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));
        }

        [TestMethod]
        public void RuleSatisifed_NotEquals()
        {
            RuleProcessingService service = new RuleProcessingService(new Mock<ICRUDService<Rule>>().Object);

            Transaction transaction = new Transaction(null, "account_id", DateTime.Now, null, "123456abcdef", 230.0M, "TestDescription");

            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.NotEquals,
                Parameter = "123456abcdefg"
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));


            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.NotEquals
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.NotEquals,
                Parameter = "123456abcdef"
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));
        }

        [TestMethod]
        public void RuleSatisifed_Contains()
        {
            RuleProcessingService service = new RuleProcessingService(new Mock<ICRUDService<Rule>>().Object);

            Transaction transaction = new Transaction(null, "account_id", DateTime.Now, null, "123456abcdef", 230.0M, "TestDescription");

            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.Contains,
                Parameter = "123456"
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));


            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.Contains,
                Parameter = "ABCDEF"
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.Contains,
                Parameter = "abcdef"
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.Contains,
                Parameter = ""
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.Contains,
                Parameter = "aabcdef"
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.Contains
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));
        }

        [TestMethod]
        public void RuleSatisifed_NotContains()
        {
            RuleProcessingService service = new RuleProcessingService(new Mock<ICRUDService<Rule>>().Object);

            Transaction transaction = new Transaction(null, "account_id", DateTime.Now, null, "123456abcdef", 230.0M, "TestDescription");

            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.NotContains,
                Parameter = "123456"
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));


            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.NotContains,
                Parameter = "ABCDEF"
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.NotContains,
                Parameter = "abcdef"
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.NotContains,
                Parameter = ""
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.NotContains,
                Parameter = "aabcdef"
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterPartyAccount,
                Operator = RuleOperator.NotContains
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));
        }

        [TestMethod]
        public void RuleSatisifed_GreaterThan()
        {
            RuleProcessingService service = new RuleProcessingService(new Mock<ICRUDService<Rule>>().Object);

            Transaction transaction = new Transaction(null, "account_id", new DateTime(2020, 5, 20), null, "123456abcdef", 230.0M, "TestDescription");

            // Parameter Greater
            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.GreaterThan,
                Parameter = 240.0M
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Date,
                Operator = RuleOperator.GreaterThan,
                Parameter = new DateTime(2020, 5, 21)
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            // Parameter Equal
            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.GreaterThan,
                Parameter = 230.0M
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Date,
                Operator = RuleOperator.GreaterThan,
                Parameter = new DateTime(2020, 5, 20)
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            // Parameter smaller
            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.GreaterThan,
                Parameter = 220.0M
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Date,
                Operator = RuleOperator.GreaterThan,
                Parameter = new DateTime(2020, 5, 19)
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            // No parameter
            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.GreaterThan
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Date,
                Operator = RuleOperator.GreaterThan
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));
        }

        [TestMethod]
        public void RuleSatisifed_GreaterThanOrEqual()
        {
            RuleProcessingService service = new RuleProcessingService(new Mock<ICRUDService<Rule>>().Object);

            Transaction transaction = new Transaction(null, "account_id", new DateTime(2020, 5, 20), null, "123456abcdef", 230.0M, "TestDescription");

            // Parameter Greater
            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.GreaterThanOrEqualTo,
                Parameter = 240.0M
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Date,
                Operator = RuleOperator.GreaterThanOrEqualTo,
                Parameter = new DateTime(2020, 5, 21)
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            // Parameter Equal
            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.GreaterThanOrEqualTo,
                Parameter = 230.0M
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Date,
                Operator = RuleOperator.GreaterThanOrEqualTo,
                Parameter = new DateTime(2020, 5, 20)
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            // Parameter smaller
            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.GreaterThanOrEqualTo,
                Parameter = 220.0M
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Date,
                Operator = RuleOperator.GreaterThanOrEqualTo,
                Parameter = new DateTime(2020, 5, 19)
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            // No parameter
            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.GreaterThanOrEqualTo
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Date,
                Operator = RuleOperator.GreaterThanOrEqualTo
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));
        }

        [TestMethod]
        public void RuleSatisifed_LessThan()
        {
            RuleProcessingService service = new RuleProcessingService(new Mock<ICRUDService<Rule>>().Object);

            Transaction transaction = new Transaction(null, "account_id", new DateTime(2020, 5, 20), null, "123456abcdef", 230.0M, "TestDescription");

            // Parameter Less
            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.LessThan,
                Parameter = 240.0M
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Date,
                Operator = RuleOperator.LessThan,
                Parameter = new DateTime(2020, 5, 21)
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            // Parameter Equal
            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.LessThan,
                Parameter = 230.0M
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Date,
                Operator = RuleOperator.LessThan,
                Parameter = new DateTime(2020, 5, 20)
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            // Parameter smaller
            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.LessThan,
                Parameter = 220.0M
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Date,
                Operator = RuleOperator.LessThan,
                Parameter = new DateTime(2020, 5, 19)
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            // No parameter
            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.LessThan
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Date,
                Operator = RuleOperator.LessThan
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));
        }

        [TestMethod]
        public void RuleSatisifed_LessThanOrEqual()
        {
            RuleProcessingService service = new RuleProcessingService(new Mock<ICRUDService<Rule>>().Object);

            Transaction transaction = new Transaction(null, "account_id", new DateTime(2020, 5, 20), null, "123456abcdef", 230.0M, "TestDescription");

            // Parameter Less
            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.LessThanOrEqualTo,
                Parameter = 240.0M
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Date,
                Operator = RuleOperator.LessThanOrEqualTo,
                Parameter = new DateTime(2020, 5, 21)
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            // Parameter Equal
            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.LessThanOrEqualTo,
                Parameter = 230.0M
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Date,
                Operator = RuleOperator.LessThanOrEqualTo,
                Parameter = new DateTime(2020, 5, 20)
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            // Parameter smaller
            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.LessThanOrEqualTo,
                Parameter = 220.0M
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Date,
                Operator = RuleOperator.LessThanOrEqualTo,
                Parameter = new DateTime(2020, 5, 19)
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            // No parameter
            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.LessThanOrEqualTo
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Date,
                Operator = RuleOperator.LessThanOrEqualTo
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));
        }

        [TestMethod]
        public void RuleSatisifed_AND()
        {
            RuleProcessingService service = new RuleProcessingService(new Mock<ICRUDService<Rule>>().Object);

            Transaction transaction1 = new Transaction(null, "account_id", new DateTime(2020, 5, 20), null, "123456abcdef", 230.0M, "TestDescription");
            Transaction transaction2 = new Transaction(null, "account_id", new DateTime(2020, 5, 21), null, "123456abcdef", 250.0M, "TestDescription");
            Transaction transaction3 = new Transaction(null, "account_id", new DateTime(2020, 5, 22), null, "123456abcdef", 270.0M, "TestDescription");
            Transaction transaction4 = new Transaction(null, "account_id", new DateTime(2020, 5, 22), null, "123456abcdef", 250.0M, "TestDescription");

            // Single AND
            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.GreaterThan,
                Parameter = 240.0M,
                RelationType = RuleRelationType.And,
                OtherSegments = new List<RuleSegment>()
                {
                    new RuleSegment()
                    {
                        Field = TransactionField.Amount,
                        Operator = RuleOperator.LessThan,
                        Parameter = 260.0M,
                    }
                }
            });

            Assert.IsFalse(service.RuleSatisifed(transaction1, rule));
            Assert.IsTrue(service.RuleSatisifed(transaction2, rule));
            Assert.IsFalse(service.RuleSatisifed(transaction3, rule));
            Assert.IsTrue(service.RuleSatisifed(transaction4, rule));

            // Multiple And
            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.GreaterThan,
                Parameter = 240.0M,
                RelationType = RuleRelationType.And,
                OtherSegments = new List<RuleSegment>()
                {
                    new RuleSegment()
                    {
                        Field = TransactionField.Amount,
                        Operator = RuleOperator.LessThan,
                        Parameter = 260.0M,
                    },
                    new RuleSegment()
                    {
                        Field = TransactionField.Date,
                        Operator = RuleOperator.LessThan,
                        Parameter = new DateTime(2020, 5, 22),
                    }
                }
            });

            Assert.IsFalse(service.RuleSatisifed(transaction1, rule));
            Assert.IsTrue(service.RuleSatisifed(transaction2, rule));
            Assert.IsFalse(service.RuleSatisifed(transaction3, rule));
            Assert.IsFalse(service.RuleSatisifed(transaction4, rule));
        }

        [TestMethod]
        public void RuleSatisifed_OR()
        {
            RuleProcessingService service = new RuleProcessingService(new Mock<ICRUDService<Rule>>().Object);

            Transaction transaction1 = new Transaction(null, "account_id", new DateTime(2020, 5, 20), null, "123456abcdef", 230.0M, "TestDescription");
            Transaction transaction2 = new Transaction(null, "account_id", new DateTime(2020, 5, 21), null, "123456abcdef", 250.0M, "TestDescription");
            Transaction transaction3 = new Transaction(null, "account_id", new DateTime(2020, 5, 22), null, "123456abcdef", 270.0M, "TestDescription");
            Transaction transaction4 = new Transaction(null, "account_id", new DateTime(2020, 5, 22), null, "123456abcdef", 250.0M, "TestDescription");

            // Single Or
            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.GreaterThan,
                Parameter = 250.0M,
                RelationType = RuleRelationType.Or,
                OtherSegments = new List<RuleSegment>()
                {
                    new RuleSegment()
                    {
                        Field = TransactionField.Amount,
                        Operator = RuleOperator.LessThan,
                        Parameter = 250.0M,
                    }
                }
            });

            Assert.IsTrue(service.RuleSatisifed(transaction1, rule));
            Assert.IsFalse(service.RuleSatisifed(transaction2, rule));
            Assert.IsTrue(service.RuleSatisifed(transaction3, rule));
            Assert.IsFalse(service.RuleSatisifed(transaction4, rule));

            // Multiple Or
            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.GreaterThan,
                Parameter = 250.0M,
                RelationType = RuleRelationType.Or,
                OtherSegments = new List<RuleSegment>()
                {
                    new RuleSegment()
                    {
                        Field = TransactionField.Amount,
                        Operator = RuleOperator.LessThan,
                        Parameter = 250.0M,
                    },
                    new RuleSegment()
                    {
                        Field = TransactionField.Date,
                        Operator = RuleOperator.Equals,
                        Parameter = new DateTime(2020,5,22),
                    }
                }
            });

            Assert.IsTrue(service.RuleSatisifed(transaction1, rule));
            Assert.IsFalse(service.RuleSatisifed(transaction2, rule));
            Assert.IsTrue(service.RuleSatisifed(transaction3, rule));
            Assert.IsTrue(service.RuleSatisifed(transaction4, rule));
        }

        [TestMethod]
        public void RuleSatisifed_Combined()
        {
            RuleProcessingService service = new RuleProcessingService(new Mock<ICRUDService<Rule>>().Object);

            Transaction transaction1 = new Transaction(null, "account_id", new DateTime(2020, 5, 20), null, "123456abcdef", 230.0M, "TestDescription");
            Transaction transaction2 = new Transaction(null, "account_id", new DateTime(2020, 5, 21), null, "123456abcdef", 250.0M, "TestDescription");
            Transaction transaction3 = new Transaction(null, "account_id", new DateTime(2020, 5, 22), null, "123456abcdef", 270.0M, "TestDescription");
            Transaction transaction4 = new Transaction(null, "account_id", new DateTime(2020, 5, 22), null, "123456abcdef", 250.0M, "TestDescription");

            // Single Or
            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.Amount,
                Operator = RuleOperator.GreaterThan,
                Parameter = 250.0M,
                RelationType = RuleRelationType.Or,
                OtherSegments = new List<RuleSegment>()
                {
                    new RuleSegment()
                    {
                        Field = TransactionField.Amount,
                        Operator = RuleOperator.LessThan,
                        Parameter = 250.0M,
                        RelationType= RuleRelationType.And,
                        OtherSegments = new List<RuleSegment>()
                        {
                            new RuleSegment()
                            {
                                Field = TransactionField.Date,
                                Operator = RuleOperator.Equals,
                                Parameter = new DateTime(2020, 5, 22)
                            }
                        }
                    }
                }
            });

            Assert.IsFalse(service.RuleSatisifed(transaction1, rule));
            Assert.IsFalse(service.RuleSatisifed(transaction2, rule));
            Assert.IsTrue(service.RuleSatisifed(transaction3, rule));
            Assert.IsFalse(service.RuleSatisifed(transaction4, rule));
        }
    }
}
