using StockAnalysis.Application.DTOs;
using StockAnalysis.Application.Services.PriceService;
using StockAnalysis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockAnalysis.Application.Indicators;
using System.Numerics;

namespace StockAnalysis.Application.Services.KLineDataProcessor
{
    public class KLineDataProcessorService : IKLineDataProcessorService
    {
        private readonly IPriceService _priceService;

        public KLineDataProcessorService(IPriceService priceService)
        {
            _priceService = priceService;
        }
        public async Task<KLineAnalysisData> PrepareData(string stockId, DateTime start, DateTime end)
        {
            List<StockDailyPrice> bufferPrices = await _priceService.GetPricesAsync(stockId, start.AddDays(-365), end, true);
            List<StockDailyPrice> prices = bufferPrices.Where(x => x.TradeDate >= start).ToList();
            int _left = bufferPrices.Count - prices.Count;


            List<decimal> bufferCloses = bufferPrices.Select(x => x.ClosePrice).ToList();
            List<long> bufferVolumes = bufferPrices.Select(x => x.Volume).ToList();

            //Dictionary<int, List<decimal?>> priceMaMap = MaDays.ToDictionary(day => day, day => Indicator.CalculateSma(preCloses, day).Skip(_left).ToList());
            //Dictionary<int, List<decimal?>> volumeMaMap = MaDays.ToDictionary(day => day, day => Indicator.CalculateSma(preVolumes, day).Skip(_left).ToList());

            //Console.WriteLine($"prices Count {prices.Count}");
            //Console.WriteLine($"buffPrices Count {buffPrices.Count}");
            //Console.WriteLine($"maMap[5] Count {priceMaMap[5].Count}");

            //Console.WriteLine($"volumeMaMap Count {volumeMaMap.Count}");
            //Console.WriteLine($"volumeMaMap[60] Count {volumeMaMap[60].Count}");




            return new KLineAnalysisData()
            {
                Prices = prices,
                PriceMAMap = CreateMaMap(bufferCloses, _left),
                VolumeMaMap = CreateMaMap(bufferVolumes, _left)


            };
        }
        private Dictionary<int, List<decimal?>> CreateMaMap<T>(List<T> numbers, int skip) where T : INumber<T>
        {
            int[] MaDays = new int[] { 5, 10, 20, 60, 120, 240 };
            return MaDays.ToDictionary(day => day, day => Indicator.CalculateSma(numbers, day).Skip(skip).ToList());
        }
    }
}
