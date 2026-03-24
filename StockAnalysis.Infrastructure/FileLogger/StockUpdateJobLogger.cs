using StockAnalysis.Application.Services.JobLogger;

namespace StockAnalysis.Infrastructure.FileLogger
{
    public class StockUpdateJobLogger : IJobLogger
    {
        private readonly FileLogger _log;
        private string _stockId;

        public StockUpdateJobLogger()
        {
            string logFile = $"Logs/StockUpdate_Single_{_stockId}_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid().ToString("N")[..5]}.log";
            _log = new FileLogger(logFile);
        }

        public void SetStockId(string stockId)
        {
            _stockId = stockId;
        }
        // ===== 共用 =====
        public void JobStart(string title)
        {
            _log.Info("==================================================");
            _log.Info(title);
            _log.Info("==================================================");
        }
        public void JobEnd(TimeSpan totalTime)
        {
            _log.Info($"Total Time: {totalTime}");
            _log.Info("==================================================");
        }
        // ===== 單筆 =====
        public void SingleStockInfo(int year)
        {
            _log.Info($"StockId : {_stockId}");
            _log.Info($"Year    : {year}");
        }

        public void SingleStockSuccess()
        {
            _log.Info($"Stock {_stockId} update success");
        }

        public void SingleStockFail(Exception ex)
        {
            _log.Error($"Stock {_stockId} update failed");
            _log.Error(ex.Message);
        }
        // ===== 批次 =====
        public void JobStart(int year, int total)
        {
            _log.Info("==================================================");
            _log.Info("Stock Update Job Started");
            _log.Info($"Year      : {year}");
            _log.Info($"TotalStock: {total}");
            _log.Info("==================================================");
        }
        public void StockStart(int index, int total)
        {
            _log.Info($"{_stockId} Start ({index}/{total})");
        }
        public void StockSuccess()
        {
            _log.Info($"{_stockId} Success");
        }
        public void StockFail(Exception? ex = null)
        {
            _log.Error($"{_stockId} Failed");
            if (ex != null)
                _log.Error($"Error: {ex.Message}");
        }
        public void RetryFail(int retry, Exception ex)
        {
            _log.Error($"Retry {retry}/3 failed for {_stockId}: {ex.Message}");
        }
        public void JobSummary(
            int success,
            int fail,
            int total,
            IEnumerable<string> failedStocks,
            DateTime startTime)
        {
            var endTime = DateTime.Now;

            _log.Info("--------------------------------------------------");
            _log.Info("---------------------Summary----------------------");
            _log.Info("--------------------------------------------------");
            _log.Info($"Total   : {total}");
            _log.Info($"Success : {success}");
            _log.Info($"Failed  : {fail}");

            if (failedStocks.Any())
            {
                _log.Info("Failed Stock List:");
                foreach (var s in failedStocks)
                    _log.Info($"- {s}");
            }

            _log.Info($"Total Time: {endTime - startTime}");
            _log.Info($"EndTime  : {endTime}");
            _log.Info("==================================================");
        }
    }
}
