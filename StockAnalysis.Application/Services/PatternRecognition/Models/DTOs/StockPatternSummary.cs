using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.Services.PatternRecognition.Models.DTOs
{
    public class StockPatternSummary
    {
        public string StockId { get; set; }
        public List<PatternMatchResult> Matches { get; set; }
    }
}
