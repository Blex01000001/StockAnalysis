using StockAnalysis.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.Services.KLine
{
    public interface IKLineChartService
    {
        KLineChartRequest PrepareKLine(string stockId, DateTime start, DateTime end);
        Task<KLineChartDto> ExecuteRequestAsync(KLineChartRequest kLineChartRequest);
    }
}
