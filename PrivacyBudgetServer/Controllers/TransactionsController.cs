using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using PrivacyBudgetServer.Models.Database;
using PrivacyBudgetServer.Services;
using System.Globalization;

namespace PrivacyBudgetServer.Controllers
{
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ILogger<TransactionsController> _logger;
        private readonly ICRUDService<Transaction> _transactionService;

        public TransactionsController(ILogger<TransactionsController> logger, ICRUDService<Transaction> transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }

        [HttpGet("[controller]")]
        public Task<List<Transaction>> Get()
        {
            return _transactionService.GetAsync();
        }

        [HttpGet("[controller]/{id:length(24)}")]
        public async Task<ActionResult<Transaction>> Get(string id)
        {
            var transaction = await _transactionService.GetAsync(id);

            if (transaction is null)
            {
                return NotFound();
            }

            return transaction;
        }

        [HttpPut("[controller]")]
        public async Task<IActionResult> Create(Transaction newTransaction)
        {
            await _transactionService.CreateAsync(newTransaction);

            return CreatedAtAction(nameof(Get), new { id = newTransaction.Id }, newTransaction);
        }

        [HttpPatch("[controller]/{id:length(24)}")]
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

        [HttpDelete("[controller]/{id:length(24)}")]
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

        [HttpPost("[controller]/Import")]
        public async Task<IActionResult> ImportBatch(
            IFormFile csvFile,
            string accountId,
            string dateHeader = "Date",
            string fromHeader = "From",
            string fromAccountHeader = "FromAccount",
            string toHeader = "To",
            string toAccountHeader = "ToAccount",
            string amountHeader = "Amount",
            string typeHeader = "Type",
            string descriptionHeader = "Description"
        )
        {
            try
            {
                List<Transaction> transactions = new List<Transaction>();

                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        bool dateFound = csv.TryGetField<DateTime>(dateHeader, out DateTime date);
                        bool fromFound = csv.TryGetField<string>(fromHeader, out string from);
                        bool fromAccountFound = csv.TryGetField<string>(fromAccountHeader, out string fromAccount);
                        bool toFound = csv.TryGetField<string>(toHeader, out string to);
                        bool toAccountFound = csv.TryGetField<string>(toAccountHeader, out string toAccount);
                        bool amountFound = csv.TryGetField<decimal>(amountHeader, out decimal amount);
                        bool typeFound = csv.TryGetField<string>(typeHeader, out string type);
                        bool descriptionFound = csv.TryGetField<string>(descriptionHeader, out string description);

                        if(!dateFound || !fromAccountFound || !amountFound)
                        {
                            return BadRequest();
                        }

                        transactions.Add(new Transaction(null, accountId, date, from, fromAccount, to, toAccount, amount, type, description));
                    }
                }

                await _transactionService.CreateManyAsync(transactions);

                return StatusCode(200);
            }
            catch(Exception ex)
            {
                return Problem(detail: ex.Message, title: "Could not read file", statusCode: 400);
            }
        }
    }
}