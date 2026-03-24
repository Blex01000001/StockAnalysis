using Microsoft.AspNetCore.SignalR;
using StockAnalysis.API.Hubs;
using StockAnalysis.Application.Services.StockUpdateNotifier;

namespace StockAnalysis.API.Notifier
{
    public class SignalRUpdateNotifier : IStockUpdateNotifier
    {
        private readonly IHubContext<StockUpdateHub> _hubContext;

        public SignalRUpdateNotifier(IHubContext<StockUpdateHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyUpdateProgress(string message)
        {
            // 呼叫真正的 SignalR Hub
            await _hubContext.Clients.All.SendAsync("Progress", message);
        }
    }
}
