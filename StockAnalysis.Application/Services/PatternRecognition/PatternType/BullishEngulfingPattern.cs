using StockAnalysis.Application.DTOs;
using StockAnalysis.Application.Services.PatternRecognition.Models.DTOs;
using StockAnalysis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.Services.PatternRecognition.PatternType
{
    public class BullishEngulfingPattern : IKLinePattern
    {
        public string Name => "BullishEngulfing";

        public IEnumerable<PatternMatchResult> Match(KLineAnalysisData analysisData)
        {
            for (int i = 1; i < analysisData.Prices.Count; i++)
            {
                KLineAnalysisContext ctx = new KLineAnalysisContext(analysisData, i);

                var T_5 = ctx.Day(-5);
                var T_1 = ctx.Day(-1);
                var T1 = ctx.Day(0);
                var T2 = ctx.Day(1);
                var T3 = ctx.Day(2);
                if (new[] { T_5, T_1, T1, T2, T3 }.Any(k => k == null)) continue;

                if (!(T_1.BodyPct < -2 && T_1.LowerShadowPct < 1 && T_1.UpperShadowPct < 1 &&
                T1.BodyPct > 4 && 
                T1.Open < T_1.Low && T1.Close > T_1.High &&

                T_5.PriceMA(10) > T1.PriceMA(10) && T1.Volume > T1.VolumeMA(10) * 2m
                )) continue;

                yield return new PatternMatchResult
                {
                    PatternName = Name,
                    StartIndex = i,
                    EndIndex = i + 1,
                    Signal = "Bullish"
                };
            }
        }
    }
}
