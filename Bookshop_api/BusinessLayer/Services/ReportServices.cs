using Bookshop_api.BusinessLayer.Interfaces;
using Bookshop_api.Data;
using Bookshop_api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookshop_api.BusinessLayer.Services
{
    public class ReportServices : IReport
    {
        private readonly ApplicationDBContext _context;

        public ReportServices(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BestSellingBookReport>> GetBestSellingBooksReportAsync()
        {
            var result = await(from orderItem in _context.OrderItems
                               join book in _context.Books on orderItem.BookId equals book.Id
                               group orderItem by new { book.Title } into g
                               select new BestSellingBookReport
                               {
                                   BookTitle = g.Key.Title,
                                   QuantitySold = g.Sum(x => x.Quantity),
                                   TotalRevenue = g.Sum(x => x.Quantity * x.TotalPrice)
                               })
                               .Take(5)
                               .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<SalesCancellationReport>> GetSalesVsCancelledReportAsync()
        {
            return await _context.Orders
                .GroupBy(o => o.Status)
                .Select(g => new SalesCancellationReport
                {
                    Status = g.Key,
                    TotalAmount = (decimal)g.Sum(o => o.TotalPrice)
                })
                .ToListAsync();
        }

        public async Task<List<StockLevelReport>> GetStockLevelsReportAsync()
        {
           var result = await _context.Books
            .Select(b => new StockLevelReport
            {
                BookTitle = b.Title,
                StockLevel = b.qty
            })
            .ToListAsync();

        return result;
        }
    }
}
