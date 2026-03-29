using StockAnalysis.Application.DTOs;
using StockAnalysis.Application.Services.PatternRecognition.Models.DTOs;
using StockAnalysis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.Application.Services.PatternRecognition
{
    public interface IKLinePattern
    {
        string Name { get; }
        // 傳入已還原的股價清單，回傳符合該型態的所有區間
        IEnumerable<PatternMatchResult> Match(KLineAnalysisData analysisData);
    }
}
