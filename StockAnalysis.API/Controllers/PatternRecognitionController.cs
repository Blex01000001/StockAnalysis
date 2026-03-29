using Microsoft.AspNetCore.Mvc;
using StockAnalysis.Application.Services.PatternRecognition;
using StockAnalysis.Application.Services.PatternRecognition.Models.DTOs;
using StockAnalysis.Application.Services.PatternRecognition.Models.Response;

namespace StockAnalysis.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatternRecognitionController : ControllerBase
    {
        private readonly IPatternRecognitionService _patternService;

        public PatternRecognitionController(IPatternRecognitionService patternService)
        {
            _patternService = patternService;
        }

        /// <summary>
        /// 取得單一股票的型態分析 (用於繪圖)
        /// </summary>
        [HttpGet("analyze/{stockId}")]
        public async Task<ActionResult<PatternAnalysisResponse>> GetAnalysis(
            string stockId,
            [FromQuery] string? start,
            [FromQuery] string? end)
        {

            var result = await _patternService.GetPatternAnalysisAsync(stockId, start, end);
            return Ok(result);
        }

        /// <summary>
        /// 掃描市場中符合特定型態的股票
        /// </summary>
        [HttpGet("scan/{patternName}")]
        public async Task<ActionResult<List<StockPatternSummary>>> ScanMarket(
            string patternName,
            [FromQuery] string? start,
            [FromQuery] string? end)
        {
            var results = await _patternService.ScanMarketPatternsAsync(patternName, start, end);
            return Ok(results);
        }
    }
}
