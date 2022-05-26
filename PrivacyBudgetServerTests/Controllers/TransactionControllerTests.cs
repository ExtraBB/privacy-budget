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

        [TestMethod]
        public async Task TestGet()
        {
            // Setup
            Mock<ICRUDService<Transaction>> transactionServiceMock = new Mock<ICRUDService<Transaction>>();
            TransactionController controller = CreateTransactionController(transactionServiceMock.Object);

            // Action
            await controller.Get();

            // Assert
            transactionServiceMock.Verify(service => service.GetAsync());
        }
    }
}
