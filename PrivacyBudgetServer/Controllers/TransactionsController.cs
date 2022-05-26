using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using PrivacyBudgetServer.Models;
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
        public async Task<IActionResult> Post(Transaction newTransaction)
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
        public async Task<IActionResult> ImportBatch(IFormFile csvFile)
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
                        bool dateFound = csv.TryGetField<DateTime>("Date", out DateTime date);
                        bool fromFound = csv.TryGetField<string>("From", out string from);
                        bool fromAccountFound = csv.TryGetField<string>("FromAccount", out string fromAccount);
                        bool toFound = csv.TryGetField<string>("To", out string to);
                        bool toAccountFound = csv.TryGetField<string>("ToAccount", out string toAccount);
                        bool amountFound = csv.TryGetField<decimal>("Amount", out decimal amount);
                        bool typeFound = csv.TryGetField<string>("Type", out string type);
                        bool descriptionFound = csv.TryGetField<string>("Description", out string description);

                        if(!dateFound || !fromAccountFound || !amountFound)
                        {
                            return BadRequest();
                        }

                        // TODO: Add way to specify header names
                        transactions.Add(new Transaction(null, date, from, fromAccount, to, toAccount, amount, type, description));
                    }
                }

                // TODO: store in database

                return StatusCode(200);
            }
            catch(Exception ex)
            {
                return Problem(detail: ex.Message, title: "Could not read file", statusCode: 400);
            }
        }
    }
}