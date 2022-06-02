﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            Transaction transaction = new Transaction(null, "account_id", DateTime.Now, "Test User", "abcdef123456", null, "123456abcdef", 230.0M, "ESF", "TestDescription");

            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.From,
                Operator = RuleOperator.Exists
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));


            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.To,
                Operator = RuleOperator.Exists
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));
        }

        [TestMethod]
        public void RuleSatisifed_NotExists()
        {
            RuleProcessingService service = new RuleProcessingService(new Mock<ICRUDService<Rule>>().Object);

            Transaction transaction = new Transaction(null, "account_id", DateTime.Now, "Test User", "abcdef123456", null, "123456abcdef", 230.0M, "ESF", "TestDescription");

            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.From,
                Operator = RuleOperator.NotExists
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));


            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.To,
                Operator = RuleOperator.NotExists
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));
        }

        [TestMethod]
        public void RuleSatisifed_Equals()
        {
            RuleProcessingService service = new RuleProcessingService(new Mock<ICRUDService<Rule>>().Object);

            Transaction transaction = new Transaction(null, "account_id", DateTime.Now, "Test User", "abcdef123456", null, "123456abcdef", 230.0M, "ESF", "TestDescription");

            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.From,
                Operator = RuleOperator.Equals,
                Parameter = "Test User"
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));


            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.From,
                Operator = RuleOperator.Equals,
                Parameter = "Test User 2"
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.From,
                Operator = RuleOperator.Equals
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));
        }

        [TestMethod]
        public void RuleSatisifed_NotEquals()
        {
            RuleProcessingService service = new RuleProcessingService(new Mock<ICRUDService<Rule>>().Object);

            Transaction transaction = new Transaction(null, "account_id", DateTime.Now, "Test User", "abcdef123456", null, "123456abcdef", 230.0M, "ESF", "TestDescription");

            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.From,
                Operator = RuleOperator.NotEquals,
                Parameter = "Test User 2"
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));


            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.From,
                Operator = RuleOperator.NotEquals
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.From,
                Operator = RuleOperator.NotEquals,
                Parameter = "Test User"
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));
        }

        [TestMethod]
        public void RuleSatisifed_Contains()
        {
            RuleProcessingService service = new RuleProcessingService(new Mock<ICRUDService<Rule>>().Object);

            Transaction transaction = new Transaction(null, "account_id", DateTime.Now, "Test User", "abcdef123456", null, "123456abcdef", 230.0M, "ESF", "TestDescription");

            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.From,
                Operator = RuleOperator.Contains,
                Parameter = "User"
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));


            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.From,
                Operator = RuleOperator.Contains,
                Parameter = "user"
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.From,
                Operator = RuleOperator.Contains,
                Parameter = " user"
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.From,
                Operator = RuleOperator.Contains,
                Parameter = ""
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.From,
                Operator = RuleOperator.Contains,
                Parameter = "uuser"
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.From,
                Operator = RuleOperator.Contains
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

            rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.To,
                Operator = RuleOperator.Contains
            });

            Assert.IsFalse(service.RuleSatisifed(transaction, rule));

        }

        // TODO: Other operators, AND, OR, Nested AND/OR
    }
}
