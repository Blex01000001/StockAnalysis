using StockAnalysis.Application.Services.PatternRecognition.Models.DTOs;
using StockAnalysis.Application.Services.PatternRecognition.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.Services.PatternRecognition
{
    public interface IPatternRecognitionService
    {
        /// <summary>
        /// 情境 1 & 2：單一股票的型態分析（回傳 K 線 + 型態標記）
        /// </summary>
        Task<PatternAnalysisResponse> GetPatternAnalysisAsync(
            string stockId,
            string? start,
            string? end);

        /// <summary>
        /// 情境 3：全市場/多檔股票掃描（僅回傳符合的股票清單與型態資訊，不含完整 K 線以節省流量）
        /// </summary>
        Task<List<StockPatternSummary>> ScanMarketPatternsAsync(
            string patternType,
            string? start,
            string? end);
    }
}
