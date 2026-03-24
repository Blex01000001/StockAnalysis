namespace StockAnalysis.Application.Services.StockUpdateNotifier
{
    public interface IStockUpdateNotifier
    {
        Task NotifyUpdateProgress(string message);
    }
}
