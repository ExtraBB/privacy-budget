using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PrivacyBudgetServer.Controllers;
using PrivacyBudgetServer.Models;
using PrivacyBudgetServer.Models.Database;
using PrivacyBudgetServer.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrivacyBudgetServerTests.Controllers
{
    [TestClass]
    public class TransactionsControllerTests
    {
        private TransactionsController CreateTransactionController(ICRUDService<Transaction> transactionService)
        {
            // Create mocks
            Mock<ILogger<TransactionsController>> mockLogger = new Mock<ILogger<TransactionsController>>();
            Mock<ICRUDService<Account>> mockAccountsService = new Mock<ICRUDService<Account>>();

            return new TransactionsController(mockLogger.Object, transactionService, mockAccountsService.Object);
        }

        private void AssertEqualTransactions(Transaction expected, Transaction actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.CounterParty, actual.CounterParty);
            Assert.AreEqual(expected.CounterPartyAccount, actual.CounterPartyAccount);
            Assert.AreEqual(expected.Amount, actual.Amount);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Category, actual.Category);
        }

        [TestMethod]
        public async Task TestGet()
        {
            // Setup
            List<Transaction> expected = new List<Transaction>()
            {
                new Transaction("TestID", "AccountId", DateTime.Now, "Person B", "IBAN B", 23.0M, "A gift to you", TransactionCategory.Gift)
            };

            Mock<ICRUDService<Transaction>> transactionServiceMock = new Mock<ICRUDService<Transaction>>();
            transactionServiceMock.Setup(ts => ts.GetAsync()).Returns(Task.FromResult(expected));
            TransactionsController controller = CreateTransactionController(transactionServiceMock.Object);

            // Action
            List<Transaction> actual = await controller.Get();

            // Assert
            transactionServiceMock.Verify(service => service.GetAsync());
            Assert.AreEqual(expected.Count, actual.Count);
            for(int i = 0; i < expected.Count; i++)
            {
                AssertEqualTransactions(expected[i], actual[i]);
            }
        }
    }
}
