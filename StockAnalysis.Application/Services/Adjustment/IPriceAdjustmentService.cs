using StockAnalysis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.Services.Adjustment
{
    public interface IPriceAdjustmentService
    {
        List<StockDailyPrice> AdjustPrices(IReadOnlyList<StockDailyPrice> prices, IReadOnlyList<StockCorporateAction> actions);
    }
}
