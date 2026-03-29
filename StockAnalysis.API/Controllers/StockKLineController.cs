using Microsoft.AspNetCore.Mvc;
using StockAnalysis.Application.DTOs;
using StockAnalysis.Application.Services.KLine;

namespace StockAnalysis.API.Controllers
{
    [ApiController]
    [Route("chart")]

    public class StockKLineController : ControllerBase
    {
        private readonly IKLineChartService _kLineChartService;
        public StockKLineController(IKLineChartService kLineChartService)
        {
            _kLineChartService = kLineChartService;

        }
        /// <summary>
        /// K 線圖
        /// </summary>
        [HttpGet("kline")]
        public async Task<IActionResult> GetKLine(
            [FromQuery] string stockId,
            [FromQuery] string? start,
            [FromQuery] string? end
        )
        {
            Console.WriteLine($"[KLine] {stockId} start={start} end={end}");

            if (string.IsNullOrWhiteSpace(stockId))
                return BadRequest("stockId is required");

            if (!string.IsNullOrWhiteSpace(start) && start.Length != 8)
                return BadRequest("start format must be yyyyMMdd");

            if (!string.IsNullOrWhiteSpace(end) && end.Length != 8)
                return BadRequest("end format must be yyyyMMdd");

            var result = await _kLineChartService.PrepareKLine(stockId, DateTime.ParseExact(start, "yyyyMMdd", null), DateTime.ParseExact(end, "yyyyMMdd", null))
                .SetMaLine()
                .SetMacd()
                .SetInstitutional()
                .SetBollingerBand()
                .SetForeignInvestmentHolding()
                .ExecuteAsync();
            return Ok(new List<KLineChartDto> { result });
        }

    }
}
