using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.DTOs
{
    public class InstitutionalPointDto
    {
        public string Date { get; set; } = default!;
        public long Buy { get; set; }
        public long Sell { get; set; }
        public long Net => Buy - Sell;
    }
}
