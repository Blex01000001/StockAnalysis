using StockAnalysis.Application.DTOs;
using StockAnalysis.Application.Services.Adjustment;
using StockAnalysis.Application.Services.KLine;
using StockAnalysis.Application.Services.KLineDataProcessor;
using StockAnalysis.Application.Services.PatternRecognition.Models.DTOs;
using StockAnalysis.Application.Services.PatternRecognition.Models.Response;
using StockAnalysis.Application.Services.PriceService;
using StockAnalysis.Domain.Entities;
using StockAnalysis.Domain.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.Services.PatternRecognition
{
    public class PatternRecognitionService : IPatternRecognitionService
    {
        private readonly IStockMetadataRepository _metadataRepo;
        private readonly IEnumerable<IKLinePattern> _patterns;
        private readonly IKLineChartService _kLineChartService;
        private readonly IPriceService _priceService;
        private readonly IKLineDataProcessorService _kLineDataProcessor;

        public PatternRecognitionService(
            IStockMetadataRepository metadataRepo,
            IEnumerable<IKLinePattern> patterns,
            IKLineChartService kLineChartService,
            IPriceService priceService,
            IKLineDataProcessorService kLineDataProcessorService)
        {
            _metadataRepo = metadataRepo;
            _patterns = patterns;
            _kLineChartService = kLineChartService;
            _priceService = priceService;
            _kLineDataProcessor = kLineDataProcessorService;
        }

        // 情境 1 & 2：單檔股票詳細分析
        public async Task<PatternAnalysisResponse> GetPatternAnalysisAsync(string stockId, string? start, string? end)
        {
            // 1. 抓取資料與還原價格 (參考 KLineChartService 邏輯)
            var prices = await _priceService.GetPricesAsync(stockId, DateTime.ParseExact(start, "yyyyMMdd", null), DateTime.ParseExact(end, "yyyyMMdd", null), true);
            KLineAnalysisData analysisData = await _kLineDataProcessor.PrepareData(stockId, DateTime.ParseExact(start, "yyyyMMdd", null), DateTime.ParseExact(end, "yyyyMMdd", null));
            var results = new List<PatternMatchResult>();
            foreach (var pattern in _patterns)
            {
                results.AddRange(pattern.Match(analysisData));
            }

            var kLineChart = await _kLineChartService.PrepareKLine(stockId, DateTime.ParseExact(start, "yyyyMMdd", null), DateTime.ParseExact(end, "yyyyMMdd", null))
                .SetBollingerBand()
                .SetMacd()
                .SetMaLine()
                .ExecuteAsync();

            return new PatternAnalysisResponse
            {
                // 這裡可以呼叫原本的 KLineChartService 取得 ChartData
                ChartData = kLineChart,
                DetectedPatterns = results
            };
        }

        // 情境 3：1000 檔股票大範圍掃描
        public async Task<List<StockPatternSummary>> ScanMarketPatternsAsync(string patternName, string? start, string? end)
        {
            List<string> stockIds = (await _metadataRepo.GetStockListAsync())
                .Select(x => x.StockId).ToList();

            //List<string> stockIds = new List<string>() { "9933" };



            var summaryList = new ConcurrentBag<StockPatternSummary>();
            var targetPattern = _patterns.FirstOrDefault(p => p.Name == patternName);

            if (targetPattern == null) return new List<StockPatternSummary>();

            // 使用 Parallel 加速處理 1000 檔股票
            await Task.Run(() =>
            {
                Parallel.ForEach(stockIds, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, async stockId =>
                {
                    // 注意：在實務上，這裡建議批次抓取資料庫以優化 I/O
                    //List<StockDailyPrice> prices = await _priceService.GetPricesAsync(stockId, start, end, true);
                    KLineAnalysisData analysisData = await _kLineDataProcessor.PrepareData(stockId, DateTime.ParseExact(start, "yyyyMMdd", null), DateTime.ParseExact(end, "yyyyMMdd", null));

                    //for (int i = 0; i < analysisData.Prices.Count; i++)
                    //{
                    //    Console.WriteLine($" {analysisData.Prices[i].TradeDate.ToString("yyyy-MM-dd")} {analysisData.Prices[i].ClosePrice} MA5: {analysisData.MAMap[5][i]}\tVolume: {analysisData.Prices[i].Volume} MA5 {analysisData.VolumeMaMap[5][i]}");
                    //}


                    var matches = targetPattern.Match(analysisData).ToList();

                    if (matches.Any())
                    {
                        summaryList.Add(new StockPatternSummary { StockId = stockId, Matches = matches });
                    }
                });
            });

            return summaryList.ToList();
        }
    }
}
