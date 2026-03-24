using StockAnalysis.Application.Services.JobLogger;
using StockAnalysis.Application.Services.StockUpdateNotifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockAnalysis.Domain.Interfaces;

namespace StockAnalysis.Application.Services.UpdateOrchestrator.Updater
{
    public abstract class BaseDataUpdater : IDataUpdater
    {
        protected readonly ITradeDataRepository _tradeDataRepository;
        protected readonly IStockUpdateNotifier _notifier;
        protected readonly IJobLogger _logger;
        protected readonly HttpClient _http;
        public abstract DataType DataType { get; }

        protected BaseDataUpdater(
            ITradeDataRepository tradeDataRepository,
            IStockUpdateNotifier notifier,
            IJobLogger logger,
            HttpClient http)
        {
            _tradeDataRepository = tradeDataRepository;
            _notifier = notifier;
            _logger = logger;
            _http = http;
        }

        public abstract Task UpdateAsync(string stockId, int year);

        protected Task ReportProgressAsync(string message)
            => _notifier.NotifyUpdateProgress( message);

        protected static string? EmptyToNull(string? s)
            => string.IsNullOrWhiteSpace(s) ? null : s;

        protected async Task<T> ExecuteWithRetryAsync<T>(
            Func<Task<T>> action,
            Func<int, Exception, Task>? onRetryFail = null,
            int maxRetry = 3,
            int delaySeconds = 5)
        {
            for (int retry = 1; retry <= maxRetry; retry++)
            {
                try
                {
                    return await action();
                }
                catch (Exception ex)
                {
                    if (onRetryFail != null)
                        await onRetryFail(retry, ex);

                    if (retry == maxRetry)
                        throw;

                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                }
            }

            throw new InvalidOperationException("ExecuteWithRetryAsync unreachable.");
        }
    }
}
