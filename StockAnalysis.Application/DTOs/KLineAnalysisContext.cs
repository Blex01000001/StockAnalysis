using StockAnalysis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.DTOs
{
    public class KLineAnalysisContext
    {
        public KLineAnalysisData AnalysisData;
        public IReadOnlyList<StockDailyPrice> Data { get; }
        public int Index { get; }
        public StockDailyPrice Current => Data[Index];
        private readonly Dictionary<int, List<decimal?>> _priceMaMap;
        private readonly Dictionary<int, List<decimal?>> _volumeMaMap;

        // ===== 原始數值 =====
        public decimal Open => Current.OpenPrice;
        public decimal Close => Current.ClosePrice;
        public decimal High => Current.HighPrice;
        public decimal Low => Current.LowPrice;
        public long Volume => Current.Volume;

        // ===== K 線結構 =====
        public decimal Body { get; }
        public decimal UpperShadow { get; }
        public decimal LowerShadow { get; }

        // ===== 比例（%） =====
        public decimal BodyPct { get; }
        public decimal UpperShadowPct { get; }
        public decimal LowerShadowPct { get; }

        public KLineAnalysisContext(KLineAnalysisData analysisData, int index)
        {
            AnalysisData = analysisData;
            _priceMaMap = analysisData.PriceMAMap;
            _volumeMaMap = analysisData.VolumeMaMap;

            Data = analysisData.Prices ?? throw new ArgumentNullException(nameof(analysisData));
            Index = index;
            if (index < 0 || index >= analysisData.Prices.Count) throw new ArgumentOutOfRangeException(nameof(index));

            var d = Current;

            Body = d.ClosePrice - d.OpenPrice;

            var bodyTop = Math.Max(d.OpenPrice, d.ClosePrice);
            var bodyBottom = Math.Min(d.OpenPrice, d.ClosePrice);

            UpperShadow = Math.Max(0, d.HighPrice - bodyTop);
            LowerShadow = Math.Max(0, bodyBottom - d.LowPrice);

            if (d.ClosePrice > 0)
            {
                BodyPct = Math.Round(Body / d.ClosePrice * 100m, 2);
                UpperShadowPct = Math.Round(UpperShadow / d.ClosePrice * 100m, 2);
                LowerShadowPct = Math.Round(LowerShadow / d.ClosePrice * 100m, 2);
            }
            else
            {
                BodyPct = 0;
                UpperShadowPct = 0;
                LowerShadowPct = 0;
            }
        }
        // ===== Index 安全操作 =====
        public bool HasPrev(int n = 1) => Index - n >= 0;
        public bool HasNext(int n = 1) => Index + n < Data.Count;
        public StockDailyPrice Prev(int n = 1) => HasPrev(n) ? Data[Index - n] : null;
        public StockDailyPrice Next(int n = 1) => HasNext(n) ? Data[Index + n] : null;
        public decimal? PriceMA(int period)
        => _priceMaMap.TryGetValue(period, out var list)
        ? list[Index]
        : null;
        public decimal? VolumeMA(int period)
        => _volumeMaMap.TryGetValue(period, out var list)
        ? list[Index]
        : null;

        public KLineAnalysisContext Day(int offset)
        {
            return Has(offset) ? new KLineAnalysisContext(AnalysisData, Index + offset) : null;
        }

        private bool Has(int offset) => Index + offset >= 0 && Index + offset < Data.Count;
            

    }
}
