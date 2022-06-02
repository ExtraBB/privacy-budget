using Microsoft.AspNetCore.Mvc;
using PrivacyBudgetServer.Models.Database;
using PrivacyBudgetServer.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrivacyBudgetServer.Controllers
{
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly ICRUDService<Account> _transactionService;

        public AccountsController(ILogger<AccountsController> logger, ICRUDService<Account> transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }

        [HttpGet("[controller]")]
        public Task<List<Account>> Get()
        {
            return _transactionService.GetAsync();
        }

        [HttpGet("[controller]/{id:length(24)}")]
        public async Task<ActionResult<Account>> Get(string id)
        {
            var transaction = await _transactionService.GetAsync(id);

            if (transaction is null)
            {
                return NotFound();
            }

            return transaction;
        }

        [HttpPut("[controller]")]
        public async Task<IActionResult> Create(Account newAccount)
        {
            await _transactionService.CreateAsync(newAccount);

            return CreatedAtAction(nameof(Get), new { id = newAccount.Id }, newAccount);
        }

        [HttpPatch("[controller]/{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Account updatedAccount)
        {
            var transaction = await _transactionService.GetAsync(id);

            if (transaction is null)
            {
                return NotFound();
            }

            updatedAccount.Id = transaction.Id;

            await _transactionService.UpdateAsync(id, updatedAccount);

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
    }
}
