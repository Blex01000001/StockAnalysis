using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.DTOs
{
    public class BollingerBandsDto
    {
        /// <summary>
        /// 中軌 (通常為 20MA)
        /// </summary>
        public List<decimal?> Mid { get; set; } = new();

        /// <summary>
        /// 上軌 (中軌 + 2倍標準差)
        /// </summary>
        public List<decimal?> Upper { get; set; } = new();

        /// <summary>
        /// 下軌 (中軌 - 2倍標準差)
        /// </summary>
        public List<decimal?> Lower { get; set; } = new();
    }
}
