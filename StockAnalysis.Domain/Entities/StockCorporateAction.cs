using StockAnalysis.Domain.Enums;

namespace StockAnalysis.Domain.Entities
{
    public class StockCorporateAction
    {
        public string StockId { get; set; }

        public CorporateActionType ActionType { get; set; }

        public DateTime ExDate { get; set; }

        public decimal? Ratio { get; set; }

        public decimal? CashAmount { get; set; }

        public string Description { get; set; }
    }
}
