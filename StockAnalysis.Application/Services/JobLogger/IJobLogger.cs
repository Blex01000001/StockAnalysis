namespace StockAnalysis.Application.Services.JobLogger
{
    public interface IJobLogger
    {
        void SetStockId(string stockId);
        void JobStart(string title);
        void JobEnd(TimeSpan totalTime);
        void SingleStockInfo(int year);
        void SingleStockSuccess();
        void SingleStockFail(Exception ex);
        void JobStart(int year, int total);
        void StockStart(int index, int total);
        void StockSuccess();
        void StockFail(Exception? ex = null);
        void RetryFail(int retry, Exception ex);
        void JobSummary(int success,int fail,int total,IEnumerable<string> failedStocks,DateTime startTime);

    }
}
