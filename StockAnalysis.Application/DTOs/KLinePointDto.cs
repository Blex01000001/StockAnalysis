using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.DTOs
{
    public class KLinePointDto
    {
        /// <summary>
        /// yyyy-MM-dd（直接給前端當 X 軸）
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// [open, close, low, high]（ECharts candlestick 規定順序）
        /// </summary>
        public decimal[] Value { get; set; }

        /// <summary>
        /// 成交量（畫下方 bar 用）
        /// </summary>
        public long Volume { get; set; }
    }
}
