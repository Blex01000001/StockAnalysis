using Microsoft.Extensions.DependencyInjection;
using StockAnalysis.Application.Services.StockUpdateNotifier;
using StockAnalysis.Application.Services.UpdateOrchestrator.Updater;
using StockAnalysis.Domain.Interfaces;

namespace StockAnalysis.Application.Services.UpdateOrchestrator
{
    public class UpdateOrchestrator : IUpdateOrchestrator
    {
        private readonly IServiceScopeFactory _scopeFactory;
        protected readonly IStockUpdateNotifier _notifier;

        public UpdateOrchestrator(
            IServiceScopeFactory scopeFactory,
            IStockUpdateNotifier notifier
            )
        {
            _scopeFactory = scopeFactory;
            _notifier = notifier;
        }
        public async Task<string> QueueUpdateJobAsync(UpdateCommandRequest command)
        {
            var jobId = Guid.NewGuid().ToString();

            // 注意：這裡使用 _ = Task.Run 是為了讓 API 立即回傳 jobId，而不阻塞等待更新完成
            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var sp = scope.ServiceProvider;
                    var stockProvider = sp.GetRequiredService<IStockMetadataRepository>();
                    var updaters = sp.GetRequiredService<IEnumerable<IDataUpdater>>();
                    var updaterMap = updaters.ToDictionary(x => x.DataType);
                    List<string> stockIds;
                    switch (command.Scope)
                    {
                        case StockScope.Single:
                            // 驗證是否有輸入代號，若無則拋出異常
                            if (string.IsNullOrWhiteSpace(command.SpecificStockId)) throw new ArgumentException("當 Scope 為 Single 時，必須提供 SpecificStockId。");
                            stockIds = new List<string> { command.SpecificStockId };
                            break;
                        case StockScope.Common:
                            stockIds = new List<string> { "0050", "2330", "006208" };
                            break;
                        case StockScope.All:

                        default:
                            var allStocks = await stockProvider.GetStockListAsync();
                            stockIds = allStocks.Select(x => x.StockId).ToList();
                            break;
                    }
                    var years = GetYears(command.Time, command.SpecificYear);
                    await ProcessUpdateAsync(jobId, updaterMap, stockIds, years, command.TargetDataTypes);
                }
                catch (Exception ex)
                {
                    await _notifier.NotifyUpdateProgress($"❌ 任務失敗 {jobId}：{ex.Message}");
                }
            });
            return jobId;
        }

        private async Task ProcessUpdateAsync(
            string jobId,
            IReadOnlyDictionary<DataType, IDataUpdater> updaterMap,
            List<string> stocks,
            List<int> years,
            List<DataType> dataTypes)
        {
            foreach (var dataType in dataTypes)
            {
                if (!updaterMap.TryGetValue(dataType, out var updater))
                {
                    Console.WriteLine($"找不到 IDataUpdater for {dataType}");
                    continue;
                }

                foreach (var stock in stocks)
                    foreach (var year in years)
                    {
                        Console.WriteLine($"{dataType} {stock} {year}");
                        await updater.UpdateAsync(stock, year);
                    }
            }

            await _notifier.NotifyUpdateProgress(
                $"✅ 任務完成 {jobId} 共 {stocks.Count} 檔股票，{years.Count} 年，{dataTypes.Count} 種資料類型"
            );
        }
        private List<int> GetYears(TimeScope scope, int? specificYear)
        {
            if (scope == TimeScope.SpecificYear)
                return new List<int> { specificYear ?? DateTime.Now.Year };

            return Enumerable.Range(1911, DateTime.Now.Year - 1911 + 1).ToList();
        }
    }
}
