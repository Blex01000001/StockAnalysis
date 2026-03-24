namespace StockAnalysis.Application.Services.UpdateOrchestrator
{
    public interface IUpdateOrchestrator
    {
        Task<string> QueueUpdateJobAsync(UpdateCommandRequest command);
    }
}
