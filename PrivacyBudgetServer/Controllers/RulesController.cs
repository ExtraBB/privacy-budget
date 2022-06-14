using Microsoft.AspNetCore.Mvc;
using PrivacyBudgetServer.Models.Database;
using PrivacyBudgetServer.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrivacyBudgetServer.Controllers
{
    [ApiController]
    public class RulesController : ControllerBase
    {
        private readonly ILogger<RulesController> _logger;
        private readonly ICRUDService<Rule> _ruleService;

        public RulesController(ILogger<RulesController> logger, ICRUDService<Rule> ruleService)
        {
            _logger = logger;
            _ruleService = ruleService;
        }

        [HttpGet("[controller]")]
        public Task<List<Rule>> Get()
        {
            return _ruleService.GetAsync();
        }

        [HttpGet("[controller]/{id:length(24)}")]
        public async Task<ActionResult<Rule>> Get(string id)
        {
            var rule = await _ruleService.GetAsync(id);

            if (rule is null)
            {
                return NotFound();
            }

            return rule;
        }

        [HttpPut("[controller]")]
        public async Task<IActionResult> Create(Rule newRule)
        {
            await _ruleService.CreateAsync(newRule);

            return CreatedAtAction(nameof(Get), new { id = newRule.Id }, newRule);
        }

        [HttpDelete("[controller]/{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var rule = await _ruleService.GetAsync(id);

            if (rule is null)
            {
                return NotFound();
            }

            // Remove rule
            await _ruleService.RemoveAsync(id);

            return NoContent();
        }
    }
}
