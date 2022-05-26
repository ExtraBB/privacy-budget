using Microsoft.AspNetCore.Mvc;
using PrivacyBudgetServer.Models;
using PrivacyBudgetServer.Services;

namespace PrivacyBudgetServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ICRUDService<Transaction> _transactionService;

        public TransactionController(ILogger<TransactionController> logger, ICRUDService<Transaction> transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }

        [HttpGet(Name = "GetTransactions")]
        public Task<List<Transaction>> Get()
        {
            return _transactionService.GetAsync();
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Transaction>> Get(string id)
        {
            var transaction = await _transactionService.GetAsync(id);

            if (transaction is null)
            {
                return NotFound();
            }

            return transaction;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Transaction newTransaction)
        {
            await _transactionService.CreateAsync(newTransaction);

            return CreatedAtAction(nameof(Get), new { id = newTransaction.Id }, newTransaction);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Transaction updatedTransaction)
        {
            var transaction = await _transactionService.GetAsync(id);

            if (transaction is null)
            {
                return NotFound();
            }

            updatedTransaction.Id = transaction.Id;

            await _transactionService.UpdateAsync(id, updatedTransaction);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var transaction = await _transactionService.GetAsync(id);

            if (transaction is null)
            {
                return NotFound();
            }

            await _transactionService.RemoveAsync(id);

            return NoContent();
        }
    }
}