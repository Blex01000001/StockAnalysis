using StockAnalysis.Application.Services.Adjustment;
using StockAnalysis.Domain.Entities;
using StockAnalysis.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.Services.PriceService
{
    public class PriceService : IPriceService
    {
        private readonly ITradeDataRepository _tradeDataRepository;
        private readonly IPriceAdjustmentService _priceAdjustmentService;

        public PriceService(ITradeDataRepository tradeDataRepository, IPriceAdjustmentService priceAdjustmentService)
        {
            _tradeDataRepository = tradeDataRepository;
            _priceAdjustmentService = priceAdjustmentService;
        }
        public async Task<List<StockDailyPrice>> GetPricesAsync(string id, DateTime start, DateTime end, bool adjusted = false)
        {
            List<StockDailyPrice> rawPrices = await _tradeDataRepository.GetDailyPricesAsync(id, start, end);
            if(!adjusted) return rawPrices;

            List<StockCorporateAction> actions = await _tradeDataRepository.GetCorporateActionsAsync(id);
            return _priceAdjustmentService.AdjustPrices(rawPrices, actions);
        }
    }
}
