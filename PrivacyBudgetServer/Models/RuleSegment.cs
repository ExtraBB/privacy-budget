using PrivacyBudgetServer.Models.Database;

namespace PrivacyBudgetServer.Models
{
    public class RuleSegment
    {
        public RuleOperator Operator { get; set; }
        public TransactionField Field { get; set; }
        public object Parameter { get; set; }

        public RuleRelationType RelationType { get; set; }
        public List<RuleSegment> OtherSegments { get; set; } = new List<RuleSegment>();

        public bool MatchesTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
            // TODO
        }
    }
}
