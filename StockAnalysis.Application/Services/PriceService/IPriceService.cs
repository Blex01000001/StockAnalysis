using StockAnalysis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.Services.PriceService
{
    public interface IPriceService
    {
        Task<List<StockDailyPrice>> GetPricesAsync(string id, string start, string end, bool adjusted = false);
    }
}
