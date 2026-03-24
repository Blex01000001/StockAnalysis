using StockAnalysis.Domain.Entities;

namespace StockAnalysis.Domain.Interfaces
{
    public interface IStockMetadataRepository
    {
        Task<List<StockInfoDto>> GetStockListAsync();
        Task<StockInfoDto> GetStockInfoAsync(string stockId);
        Task UpsertStockListAsync(List<StockInfoDto> stocks);
    }
}
