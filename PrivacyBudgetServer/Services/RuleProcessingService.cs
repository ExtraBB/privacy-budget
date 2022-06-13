using PrivacyBudgetServer.Models;
using PrivacyBudgetServer.Models.Database;

namespace PrivacyBudgetServer.Services
{
    public class RuleProcessingService
    {
        ICRUDService<Rule> _ruleService;

        public RuleProcessingService(ICRUDService<Rule> ruleService)
        {
            _ruleService = ruleService;
        }

        public async void ProcessTransactionRules(Transaction transaction)
        {
            List<Rule> rules = await _ruleService.GetAsync((Rule rule) => rule.AccountId == transaction.AccountId);
            foreach (Rule rule in rules)
            {
                if (RuleSatisifed(transaction, rule))
                {
                    transaction.Category = rule.TransactionCategory;
                    return;
                }
            }
        }

        public async void ProcessTransactionRulesBatch(Account account, List<Transaction> transactions)
        {
            List<Rule> rules = await _ruleService.GetAsync((Rule rule) => rule.AccountId == account.Id);

            foreach (Transaction transaction in transactions)
            {
                foreach (Rule rule in rules)
                {
                    if (RuleSatisifed(transaction, rule))
                    {
                        transaction.Category = rule.TransactionCategory;
                        break;
                    }
                }
            }
        }

        public bool RuleSatisifed(Transaction transaction, Rule rule)
        {
            return SegmentSatisfied(transaction, rule.Segment);
        }

        private bool SegmentSatisfied(Transaction transaction, RuleSegment segment)
        {
            bool segmentSatisfied = SegmentConditionSatisfied(transaction, segment);

            switch(segment.RelationType)
            {
                case RuleRelationType.And:
                    {
                        foreach (RuleSegment otherSegment in segment.OtherSegments)
                        {
                            segmentSatisfied &= SegmentSatisfied(transaction, otherSegment);
                        }
                        break;
                    }
                case RuleRelationType.Or:
                    {
                        foreach (RuleSegment otherSegment in segment.OtherSegments)
                        {
                            segmentSatisfied |= SegmentSatisfied(transaction, otherSegment);
                        }
                        break;
                    }
            }

            return segmentSatisfied;
        }

        private bool SegmentConditionSatisfied(Transaction transaction, RuleSegment segment)
        {
            dynamic? data = GetFieldData(transaction, segment.Field);
            dynamic parameter = segment.Parameter;

            string? dataAsString = data as string;

            switch(segment.Operator)
            {
                case RuleOperator.Exists: return data != null;
                case RuleOperator.NotExists: return data == null;
                case RuleOperator.Equals: return data == parameter;
                case RuleOperator.NotEquals: return data != parameter;
                case RuleOperator.Contains: return parameter != null && dataAsString != null && dataAsString.ToLowerInvariant().Contains(parameter.ToLowerInvariant());
                case RuleOperator.NotContains: return parameter != null && dataAsString != null && !dataAsString.ToLowerInvariant().Contains(parameter.ToLowerInvariant());
                case RuleOperator.GreaterThan: return parameter != null && data > parameter;
                case RuleOperator.GreaterThanOrEqualTo: return parameter != null && data >= parameter;
                case RuleOperator.LessThan: return parameter != null && data < parameter;
                case RuleOperator.LessThanOrEqualTo: return parameter != null && data <= parameter;
                default: return false;
            }
        }

        private dynamic? GetFieldData(Transaction transaction, string field)
        {
            return transaction.GetType().GetProperty(field)?.GetValue(transaction, null);
        }
    }
}
