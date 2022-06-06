using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PrivacyBudgetServer.Models;
using PrivacyBudgetServer.Models.Database;
using PrivacyBudgetServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.CounterParty,
                Operator = RuleOperator.Contains
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

        }

        // TODO: Other operators, AND, OR, Nested AND/OR
    }
}
