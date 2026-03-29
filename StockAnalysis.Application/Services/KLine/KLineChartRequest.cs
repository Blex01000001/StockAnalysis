using StockAnalysis.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.Services.KLine
{
    public class KLineChartRequest
    {
        private readonly IKLineChartService _service; // 回頭呼叫 Service 執行私有邏輯
        private readonly string _stockId;
        private readonly DateTime _start;
        private readonly DateTime _end;

        // 功能開關
        private bool _includeMa = false;
        private bool _includeMacd = false;
        private bool _includeBollinger = false;
        private bool _includeHoldings = false;
        private bool _includeInstitutional = false;
        private bool _includeRsi = false;

        // 使用 internal，讓 Service 可以讀取，但外部 Controller 看不到
        internal bool IncludeMa 
        { 
            get { return _includeMa; } 
            private set { _includeMa = value; } 
        }
        internal bool IncludeMacd 
        { 
            get { return _includeMacd; } 
            private set { _includeMacd = value; } 
        }
        internal bool IncludeBollinger 
        { 
            get { return _includeBollinger; } 
            private set { _includeBollinger = value; } 
        }
        internal bool IncludeHoldings 
        { 
            get { return _includeHoldings; } 
            private set { _includeHoldings = value; } 
        }
        internal bool IncludeInstitutional
        {
            get { return _includeInstitutional; }
            private set { _includeInstitutional = value; }
        }
        internal bool IncludeRsi
        {
            get { return _includeRsi; }
            private set { _includeRsi = value; }
        }
        public KLineChartRequest(IKLineChartService service, string stockId, DateTime start, DateTime end)
        {
            _service = service;
            _stockId = stockId;
            _start = start;
            _end = end;
        }
        public KLineChartRequest SetRsi()
        {
            _includeRsi = true;
            return this;
        }
        public KLineChartRequest SetMaLine()
        {
            _includeMa = true;
            return this;
        }
        public KLineChartRequest SetMacd() 
        { 
            _includeMacd = true; 
            return this; 
        }
        public KLineChartRequest SetBollingerBand() 
        { 
            _includeBollinger = true; 
            return this; 
        }
        public KLineChartRequest SetForeignInvestmentHolding() 
        { 
            _includeHoldings = true; 
            return this; 
        }
        public KLineChartRequest SetInstitutional() 
        { 
            _includeInstitutional = true; 
            return this; 
        }

        // C# 專家級寫法：解構子 (Deconstructor)
        // 這就是你問的 GetParams() 的最優化替代方案
        public void Deconstruct(out string stockId, out DateTime start, out DateTime end)
        {
            stockId = _stockId;
            start = _start;
            end = _end;
        }
        // 真正的執行點
        public Task<KLineChartDto> ExecuteAsync()
        {
            return _service.ExecuteRequestAsync(this);
        }
    }
}
