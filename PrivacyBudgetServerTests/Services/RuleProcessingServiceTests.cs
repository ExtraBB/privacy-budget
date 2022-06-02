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

            Transaction transaction = new Transaction(null, "account_id", DateTime.Now, "Test User", "abcdef123456", "Test User 2", "123456abcdef", 230.0M, "ESF", "TestDescription");

            Rule rule = new Rule("account_id", new RuleSegment()
            {
                Field = TransactionField.From,
                Operator = RuleOperator.Exists
            });

            Assert.IsTrue(service.RuleSatisifed(transaction, rule));
        }
    }
}
