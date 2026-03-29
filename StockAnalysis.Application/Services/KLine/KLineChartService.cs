using SqlKata;
using StockAnalysis.Application.DTOs;
using StockAnalysis.Application.Services.Adjustment;
using StockAnalysis.Application.Services.KLine.Builders;
using StockAnalysis.Application.Services.PriceService;
using StockAnalysis.Domain.Entities;
using StockAnalysis.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.Services.KLine
{
    public class KLineChartService : IKLineChartService
    {
        private readonly IInstitutionalInvestorRepository _institutionalInvestorRepository;
        private readonly IStockMetadataRepository _stockMetadataRepository;
        private readonly IPriceService _priceService;
        private readonly IInstitutionalInvestorsAdjustmentService _insInvAdjService;
        private readonly ITradeDataRepository _tradeDataRepository;

        public KLineChartService(
            IPriceService priceService,
            IInstitutionalInvestorsAdjustmentService insInvAdjService,
            IInstitutionalInvestorRepository institutionalInvestorRepository,
            IStockMetadataRepository stockMetadataRepository,
            ITradeDataRepository tradeDataRepository
            )
        {
            _priceService = priceService;
            _insInvAdjService = insInvAdjService;
            _stockMetadataRepository = stockMetadataRepository;
            _institutionalInvestorRepository = institutionalInvestorRepository;
            _tradeDataRepository = tradeDataRepository;
        }

        // 1. 進入點：不再直接回傳資料，而是回傳一個「配置器」
        public KLineChartRequest PrepareKLine(string stockId, DateTime start, DateTime end)
        {
            return new KLineChartRequest(this, stockId, start, end);
        }

        // 2. 內部核心邏輯：真正的「按需撈取」發生在這裡
        public async Task<KLineChartDto> ExecuteRequestAsync(KLineChartRequest req)
        {
            var (stockId, start, end) = req; // 假設有輔助方法取得參數
            //DateTime requestedStart = DateTime.ParseExact(start, "yyyyMMdd", CultureInfo.InvariantCulture);
            var bufferStart = start.AddDays(-365);


            // --- 核心優化：Task 列表 ---
            var tasks = new List<Task>();

            // 永遠需要的資料
            var actionsTask = _tradeDataRepository.GetCorporateActionsAsync(stockId);
            var pricesTask = _tradeDataRepository.GetDailyPricesAsync(stockId, bufferStart, end);

            // 按需加入的 Task
            Task<List<StockShareholding>>? holdingsTask = req.IncludeHoldings
                ? _institutionalInvestorRepository.GetShareHoldingsAsync(stockId, bufferStart, end) : null;

            Task<List<StockInstitutionalInvestorsBuySell>>? institutionalTask = req.IncludeInstitutional
                ? _institutionalInvestorRepository.GetDailyTradesAsync(stockId, bufferStart, end) : null;

            // 並行執行所有被啟動的 Task
            await Task.WhenAll(new[] { (Task)actionsTask, pricesTask }
                .Concat(new[] { (Task?)holdingsTask, institutionalTask }.Where(t => t != null)!));

            var adjPricesAll = await _priceService.GetPricesAsync(stockId, bufferStart, end, true);
            int startIndex = adjPricesAll.FindIndex(x => x.TradeDate >= start);

            if (startIndex == -1)
                throw new InvalidOperationException("No trading data found for the requested period."); // 處理找不到資料的情況（例如該股票在該區間尚未上市）

            StockInfoDto stockInfo = await _stockMetadataRepository.GetStockInfoAsync(stockId);
            var builder = new KLineChartBuilder(stockId, stockInfo.CompanyShortName, pricesTask.Result, startIndex);

            if (req.IncludeMa) builder.SetMaLine();
            if (req.IncludeMacd) builder.SetMacd();
            if (req.IncludeBollinger) builder.SetBollingerBand();
            if (holdingsTask != null) builder.SetForeignInvestmentHolding(holdingsTask.Result);
            if (institutionalTask != null) builder.SetInstitutional(institutionalTask.Result);

            return builder.Build();
        }

        //public async Task<List<KLineChartDto>> GetKLineAsyncOLD(string stockId, int? days, string? start, string? end)
        //{
        //    string stockName = _stockProvider.GetName(stockId);

        //    // 取得分割資料
        //    Query actionQuery = new Query("StockCorporateAction")
        //        .Select("StockId", "ActionType", "ExDate", "Ratio", "CashAmount", "Description")
        //        .Where("StockId", stockId)
        //        .OrderByDesc("ExDate");
        //    List<StockCorporateAction> actions = await _repo.GetCorporateActionsAsync(actionQuery);

        //    DateTime requestedStart = DateTime.ParseExact(start, "yyyyMMdd", CultureInfo.InvariantCulture);
        //    string bufferStart = requestedStart.AddDays(-365).ToString("yyyyMMdd");

        //    Query allPriceQuery = StockDailyPriceQueryBuilder.Build(stockId, null, bufferStart, end);
        //    var rawPrices = await _repo.GetPricesAsync(allPriceQuery);

        //    // 確保資料是按日期排序的 (如果 SQL 沒做，這裡要做一次)
        //    var sortedPrices = rawPrices.OrderBy(x => x.TradeDate).ToList();

        //    // 執行除權息調整邏輯
        //    var adjPricesAll = _priceAdjService.AdjustPrices(sortedPrices, actions).ToList();

        //    // 4. 關鍵優化：尋找「第一個大於等於請求日期」的索引
        //    // 這樣即使 start 是週六，它也會自動定位到下週一的資料
        //    int startIndex = adjPricesAll.FindIndex(x => x.TradeDate >= requestedStart);

        //    if (startIndex == -1)
        //    {
        //        // 處理找不到資料的情況（例如該股票在該區間尚未上市）
        //        throw new InvalidOperationException("No trading data found for the requested period.");
        //    }


        //    Query ShareholdingQuery = new Query("StockShareholding").Where("StockId", stockId);
        //    List<StockShareholding> StockShareholdings = (await _repo.GetShareHoldingsAsync(ShareholdingQuery))
        //        .OrderBy(x => x.Date)
        //        .ToList();

        //    Query InstitutionalQuery = new Query("StockInstitutionalInvestorsBuySell").Where("StockId", stockId);
        //    List<StockInstitutionalInvestorsBuySell> insInvBuySells = (await _repo.GetInstitutionalInvestorsBuySellAsync(InstitutionalQuery))
        //        .OrderBy(x => x.Date)
        //        .ToList();
        //    var adjInsInvBuySells = _insInvAdjService.AdjustBuySell(insInvBuySells, actions).ToList();

        //    return new List<KLineChartDto> {
        //        new KLineChartBuilder(stockId, stockName, adjPricesAll, startIndex)
        //            .SetMaLine()
        //            .SetBollingerBand()
        //            .SetForeignInvestmentHolding(StockShareholdings)
        //            .SetInstitutional(adjInsInvBuySells)
        //            .SetMacd()
        //            .SetRsi()
        //            .Build()
        //    };
        //}
    }
}
