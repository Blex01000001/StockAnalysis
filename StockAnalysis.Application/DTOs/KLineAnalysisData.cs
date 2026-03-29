using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockAnalysis.Domain.Entities;


namespace StockAnalysis.Application.DTOs
{
    public class KLineAnalysisData
    {
        public List<StockDailyPrice> Prices { get; set; }
        public Dictionary<int, List<decimal?>> PriceMAMap { get; set; }
        public Dictionary<int, List<decimal?>> VolumeMaMap { get; set; }




    }
}
