namespace StockAnalysis.Application.Services.UpdateOrchestrator.Updater
{
    public interface IDataUpdater
    {
        DataType DataType { get; }
        Task UpdateAsync(string stockId, int year);
    }
}
