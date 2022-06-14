using Microsoft.AspNetCore.Mvc;
using PrivacyBudgetServer.Models.Database;
using PrivacyBudgetServer.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrivacyBudgetServer.Controllers
{
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly ILogger<BudgetsController> _logger;
        private readonly ICRUDService<Budget> _budgetService;

        public BudgetsController(ILogger<BudgetsController> logger, ICRUDService<Budget> budgetService)
        {
            _logger = logger;
            _budgetService = budgetService;
        }

        [HttpGet("[controller]")]
        public Task<List<Budget>> Get()
        {
            return _budgetService.GetAsync();
        }

        [HttpGet("[controller]/{id:length(24)}")]
        public async Task<ActionResult<Budget>> Get(string id)
        {
            var budget = await _budgetService.GetAsync(id);

            if (budget is null)
            {
                return NotFound();
            }

            return budget;
        }

        [HttpPut("[controller]")]
        public async Task<IActionResult> Create(Budget newBudget)
        {
            await _budgetService.CreateAsync(newBudget);

            return CreatedAtAction(nameof(Get), new { id = newBudget.Id }, newBudget);
        }

        [HttpPatch("[controller]/{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Budget updatedBudget)
        {
            var budget = await _budgetService.GetAsync(id);

            if (budget is null)
            {
                return NotFound();
            }

            updatedBudget.Id = budget.Id;

            await _budgetService.UpdateAsync(id, updatedBudget);

            return NoContent();
        }

        [HttpDelete("[controller]/{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var budget = await _budgetService.GetAsync(id);

            if (budget is null)
            {
                return NotFound();
            }

            // Remove budget
            await _budgetService.RemoveAsync(id);

            return NoContent();
        }
    }
}
