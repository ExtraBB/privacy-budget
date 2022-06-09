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
        private readonly ICRUDService<Account> _accountService;
        private readonly ICRUDService<Transaction> _transactionService;

        public AccountsController(ILogger<AccountsController> logger, ICRUDService<Account> accountService, ICRUDService<Transaction> transactionService)
        {
            _logger = logger;
            _accountService = accountService;
            _transactionService = transactionService;
        }

        [HttpGet("[controller]")]
        public Task<List<Account>> Get()
        {
            return _accountService.GetAsync();
        }

        [HttpGet("[controller]/{id:length(24)}")]
        public async Task<ActionResult<Account>> Get(string id)
        {
            var account = await _accountService.GetAsync(id);

            if (account is null)
            {
                return NotFound();
            }

            return account;
        }

        [HttpPut("[controller]")]
        public async Task<IActionResult> Create(Account newAccount)
        {
            await _accountService.CreateAsync(newAccount);

            return CreatedAtAction(nameof(Get), new { id = newAccount.Id }, newAccount);
        }

        [HttpPatch("[controller]/{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Account updatedAccount)
        {
            var account = await _accountService.GetAsync(id);

            if (account is null)
            {
                return NotFound();
            }

            updatedAccount.Id = account.Id;

            await _accountService.UpdateAsync(id, updatedAccount);

            return NoContent();
        }

        [HttpDelete("[controller]/{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var account = await _accountService.GetAsync(id);

            if (account is null)
            {
                return NotFound();
            }

            // Remove account
            await _accountService.RemoveAsync(id);

            // Remove associated transactions
            await _transactionService.RemoveWhereAsync(t => t.AccountId == account.Id);
            

            return NoContent();
        }
    }
}
