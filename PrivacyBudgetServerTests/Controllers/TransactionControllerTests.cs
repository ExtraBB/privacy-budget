using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Moq;
using PrivacyBudgetServer.Controllers;
using PrivacyBudgetServer.Models;
using PrivacyBudgetServer.Services;
using PrivacyBudgetServerTests.Controllers.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivacyBudgetServerTests.Controllers
{
    [TestClass]
    public class TransactionControllerTests
    {
        private TransactionController CreateTransactionController(ICRUDService<Transaction> transactionService)
        {
            // Create mocks
            Mock<ILogger<TransactionController>> mockLogger = new Mock<ILogger<TransactionController>>();

            return new TransactionController(mockLogger.Object, transactionService);
        }

        private void AssertEqualTransactions(Transaction expected, Transaction actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Date, actual.Date);
        }

        [TestMethod]
        public async Task TestGet()
        {
            // Setup
            List<Transaction> expected = new List<Transaction>()
            {
                new Transaction(){ Date = DateTime.Now, Id = "ID_123" }
            };

            Mock<ICRUDService<Transaction>> transactionServiceMock = new Mock<ICRUDService<Transaction>>();
            transactionServiceMock.Setup(ts => ts.GetAsync()).Returns(Task.FromResult(expected));
            TransactionController controller = CreateTransactionController(transactionServiceMock.Object);

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
