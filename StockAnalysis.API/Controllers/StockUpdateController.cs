using Microsoft.AspNetCore.Mvc;
using StockAnalysis.Application.Services.UpdateOrchestrator;

namespace StockAnalysis.API.Controllers
{
    [ApiController]
    [Route("stock")]
    public class StockUpdateController : ControllerBase
    {
        private readonly IUpdateOrchestrator _orchestrator;
        public StockUpdateController(IUpdateOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }
        // 5. 接收 HTML
        [HttpGet("")]
        public IActionResult Index()
        {
            return PhysicalFile(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/index.html"),
                "text/html");
        }

        [HttpPost("execute")]
        public async Task<IActionResult> ExecuteUpdate([FromBody] UpdateCommandRequest command)
        {
            // 由於更新可能很久，通常會回傳一個 Job ID，而不是等待結果完成
            string jobId = await _orchestrator.QueueUpdateJobAsync(command);
            return Accepted(new { JobId = jobId });
        }
    }
}
