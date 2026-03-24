using StockAnalysis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.Services.Adjustment
{
    public interface IInstitutionalInvestorsAdjustmentService
    {
        IReadOnlyList<StockInstitutionalInvestorsBuySell> AdjustBuySell(
            IReadOnlyList<StockInstitutionalInvestorsBuySell> buySellData,
            IReadOnlyList<StockCorporateAction> actions);
    }
}
