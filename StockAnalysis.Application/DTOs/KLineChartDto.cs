using StockAnalysis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.DTOs
{
    public class KLineChartDto
    {
        public string StockId { get; set; }
        public string StockName { get; set; }
        public List<KLinePointDto> Points { get; set; } = new();
        public string PatternType { get; set; }
        public List<MALineDto> MALines { get; set; } = new();
        public List<KLineMarkerDto> Markers { get; set; } = new();
        public List<KLineMarkLineDto> MarkLines { get; set; } = new();
        public List<StockShareholding> Shareholdings { get; set; } = new();
        public InstitutionalSeriesDto Institutional { get; set; } = new();
        public MacdDto Macd { get; set; } = new();
        public BollingerBandsDto BollingerBands { get; set; } = new();
        public RsiDto Rsi { get; set; } = new();
    }
}
