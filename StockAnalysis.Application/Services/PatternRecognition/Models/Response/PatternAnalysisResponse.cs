using StockAnalysis.Application.DTOs;
using StockAnalysis.Application.Services.PatternRecognition.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.Services.PatternRecognition.Models.Response
{
    public class PatternAnalysisResponse
    {
        // 基礎 K 線資料，用於繪製底圖
        public KLineChartDto ChartData { get; set; }

        // 辨識出的型態清單，包含在 ChartData 中的索引位置
        public List<PatternMatchResult> DetectedPatterns { get; set; }
    }
}
