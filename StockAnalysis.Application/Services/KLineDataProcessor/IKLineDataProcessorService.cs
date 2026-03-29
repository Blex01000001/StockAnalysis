using StockAnalysis.Application.DTOs;
using StockAnalysis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.Services.KLineDataProcessor
{
    public interface IKLineDataProcessorService
    {
        public Task<KLineAnalysisData> PrepareData(string stockId, DateTime start, DateTime end);
    }
}
