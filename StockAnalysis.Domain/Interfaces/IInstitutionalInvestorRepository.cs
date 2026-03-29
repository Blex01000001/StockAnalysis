using StockAnalysis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Domain.Interfaces
{
    public interface IInstitutionalInvestorRepository
    {
        Task<List<StockInstitutionalInvestorsBuySell>> GetDailyTradesAsync(string stockId, DateTime start, DateTime end);
        Task UpsertDailyTradesAsync(List<StockInstitutionalInvestorsBuySell> trades);
        Task<List<StockShareholding>> GetShareHoldingsAsync(string stockId, DateTime start, DateTime end);
        Task UpsertGetShareHoldingsAsync(List<StockShareholding> ownerships);
    }
}
