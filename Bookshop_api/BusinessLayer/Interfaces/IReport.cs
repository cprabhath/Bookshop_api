using Bookshop_api.Models;

namespace Bookshop_api.BusinessLayer.Interfaces
{
    public interface IReport
    {
        Task<IEnumerable<BestSellingBookReport>> GetBestSellingBooksReportAsync();

        Task<IEnumerable<SalesCancellationReport>> GetSalesVsCancelledReportAsync();

        Task<List<StockLevelReport>> GetStockLevelsReportAsync();
    }
}
