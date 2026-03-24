using StockAnalysis.Domain.Entities;
using SqlKata;

namespace StockAnalysis.Domain.Interfaces
{
    public interface ITradeDataRepository
    {
        Task<List<StockDailyPrice>> GetDailyPricesAsync(string stockId, string start, string end);
        Task UpsertDailyPricesAsync(List<StockDailyPrice> prices);
        Task<List<StockDividend>> GetDividendsAsync(Query query);
        Task UpsertDividendsAsync(List<StockDividend> dividends);
        Task<List<StockCorporateAction>> GetCorporateActionsAsync(string stockId);
        Task UpsertCorporateActionsAsync(List<StockCorporateAction> actions);
    }
}
