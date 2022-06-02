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
            dynamic? parameter = GetParameterData(segment.Field, segment.Parameter);

            switch(segment.Operator)
            {
                case RuleOperator.Exists: return data != null;
                case RuleOperator.NotExists: return data == null;
                case RuleOperator.Equals: return data == parameter;
                case RuleOperator.NotEquals: return data != parameter;
                case RuleOperator.Contains: return data is string s1 && s1.Contains(parameter);
                case RuleOperator.NotContains: return data is string s2 && !s2.Contains(parameter);
                case RuleOperator.GreaterThan: return data > parameter;
                case RuleOperator.GreaterThanOrEqualTo: return data >= parameter;
                case RuleOperator.LessThan: return data < parameter;
                case RuleOperator.LessThanOrEqualTo: return data <= parameter;
                default: return false;
            }
        }

        private dynamic? GetFieldData(Transaction transaction, TransactionField field)
        {
            switch(field)
            {
                case TransactionField.Date: return transaction.Date;
                case TransactionField.From: return transaction.From;
                case TransactionField.FromAccount: return transaction.FromAccount;
                case TransactionField.To: return transaction.To;
                case TransactionField.ToAccount: return transaction.ToAccount;
                case TransactionField.Type: return transaction.Type;
                case TransactionField.Description: return transaction.Description;
                case TransactionField.Amount: return transaction.Amount;
                default: return null;
            }
        }

        private dynamic? GetParameterData(TransactionField field, object parameter)
        {
            switch (field)
            {
                case TransactionField.Date: return (DateTime)parameter;
                case TransactionField.From: return (string)parameter;
                case TransactionField.FromAccount: return (string)parameter;
                case TransactionField.To: return (string)parameter;
                case TransactionField.ToAccount: return (string)parameter;
                case TransactionField.Type: return (string)parameter;
                case TransactionField.Description: return (string)parameter;
                case TransactionField.Amount: return (decimal)parameter;
                default: return null;
            }
        }
    }
}
