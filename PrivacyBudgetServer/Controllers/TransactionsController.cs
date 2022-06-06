using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using PrivacyBudgetServer.Models.Database;
using PrivacyBudgetServer.Parsers;
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
                TransactionCSVParser parser = new TransactionCSVParser(accountId, new Parsers.Options.TransactionCSVParserOptions()
                {
                    HasHeaderRow = hasHeaderRow,
                    DateFormat = dateFormat,
                    DateColumn = dateColumn,
                    CounterPartyColumn = counterPartyColumn,
                    CounterPartyAccountColumn = counterPartyAccountColumn,
                    AmountColumn = amountColumn,
                    DescriptionColumn = descriptionColumn
                });

                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    List<Transaction> transactions = parser.TryParseAll(csv);

                    if (transactions.Any())
                    {
                        await _transactionService.CreateManyAsync(transactions);
                        return Ok($"Successfully imported {transactions.Count} transactions.");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch(Exception ex)
            {
                return Problem(detail: ex.Message, title: "Could not read file", statusCode: 400);
            }
        }
    }
}