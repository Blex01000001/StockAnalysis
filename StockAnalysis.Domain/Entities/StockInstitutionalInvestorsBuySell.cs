namespace StockAnalysis.Domain.Entities
{
    public class StockInstitutionalInvestorsBuySell
    {
        public string StockId { get; set; } = default!;
        public string Date { get; set; } = default!;
        public string Name { get; set; } = default!;

        public long Buy { get; set; }
        public long Sell { get; set; }

        public long NetBuySell => Buy - Sell;
    }
}
