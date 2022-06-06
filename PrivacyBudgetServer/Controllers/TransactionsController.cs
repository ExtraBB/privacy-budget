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
            [FromForm] string accountId,
            [FromForm] bool hasHeaderRow = false,
            [FromForm] string? dateFormat = "dd-MM-yyyy",
            [FromForm] int dateColumn = -1,
            [FromForm] int counterPartyColumn = -1,
            [FromForm] int counterPartyAccountColumn = -1,
            [FromForm] int amountColumn = -1,
            [FromForm] int descriptionColumn = -1
        )
        {
            try
            {
                List<Transaction> transactions = new List<Transaction>();
                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();

                    if (hasHeaderRow)
                    {
                        csv.ReadHeader();
                    }

                    while (csv.Read())
                    {
                        bool dateFound = csv.TryGetField<string>(dateColumn, out string dateString);
                        bool counterPartyFound = csv.TryGetField<string>(counterPartyColumn, out string counterParty);
                        bool counterPartyAccountFound = csv.TryGetField<string>(counterPartyAccountColumn, out string counterPartyAccount);
                        bool amountFound = csv.TryGetField<decimal>(amountColumn, out decimal amount);
                        bool descriptionFound = csv.TryGetField<string>(descriptionColumn, out string description);

                        if(!dateFound || !amountFound)
                        {
                            return BadRequest();
                        }

                        bool dateParsed = DateTime.TryParseExact(dateString, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
                        if(!dateParsed)
                        {
                            return BadRequest();
                        }
                        transactions.Add(new Transaction(null, accountId, date, counterParty, counterPartyAccount, amount, description));
                    }
                }

                await _transactionService.CreateManyAsync(transactions);

                return Ok($"Successfully imported {transactions.Count} transactions.");
            }
            catch(Exception ex)
            {
                return Problem(detail: ex.Message, title: "Could not read file", statusCode: 400);
            }
        }
    }
}