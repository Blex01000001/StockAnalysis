using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.DTOs
{
    public class RsiDto
    {
        public List<double?> Rsi6 { get; set; } = new();
        public List<double?> Rsi12 { get; set; } = new();
        public List<double?> Rsi24 { get; set; } = new();
    }
}
