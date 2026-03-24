namespace StockAnalysis.Application.Services.UpdateOrchestrator.Models.Responses
{
    internal class TwseStockDayResponse
    {
        public string stat { get; set; }
        public string date { get; set; }
        public List<List<string>> data { get; set; }
    }
}
