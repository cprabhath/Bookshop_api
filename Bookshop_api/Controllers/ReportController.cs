using Bookshop_api.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReport _reportService;

        public ReportController(IReport reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("best-selling-books")]
        public async Task<IActionResult> GetBestSellingBooks()
        {
            var data = await _reportService.GetBestSellingBooksReportAsync();

            var chartData = new
            {
                labels = data.Select(x => x.BookTitle).ToArray(),
                datasets = new object[] 
                {
            new
            {
                label = "Total Revenue",
                data = data.Select(x => x.TotalRevenue).ToArray(),
                borderColor = "rgb(54, 162, 235)",
                backgroundColor = "rgba(54, 162, 235, 0.5)"
            }
                }
            };

            return Ok(chartData);
        }

        [HttpGet("sales-vs-cancelled")]
        public async Task<IActionResult> GetSalesVsCancelled()
        {
            var data = await _reportService.GetSalesVsCancelledReportAsync();

            var chartData = new
            {
                labels = data.Select(x => x.Status).ToArray(),
                datasets = new object[]
                {
                new
                {
                    label = "Sales vs Cancelled",
                    data = data.Select(x => x.TotalAmount).ToArray(),
                    borderColor = new[] {
                        "rgb(75, 192, 192)",
                        "rgb(255, 99, 132)",
                        "rgb(54, 162, 235)"
                    },
                    backgroundColor = new[] {
                        "rgba(75, 192, 192, 0.5)",
                        "rgba(255, 99, 132, 0.5)",
                        "rgba(54, 162, 235, 0.5)"
                        }
                    }
                }
            };

            return Ok(chartData);
        }

        [HttpGet("stock-levels")]
        public async Task<IActionResult> GetStockLevels()
        {
            var data = await _reportService.GetStockLevelsReportAsync();

            var chartData = new
            {
                labels = data.Select(x => x.BookTitle).ToArray(), 
                datasets = new[]
                {
                new
                {
                    label = "Stock Levels",
                    data = data.Select(x => x.StockLevel).ToArray(), 
                    backgroundColor = new[]
                    {
                        "rgb(75, 192, 192)", 
                    },
                    borderColor = new[]
                    {
                        "rgb(54, 162, 235)", 
                    },
                    borderWidth = 1
                }
            }
            };

            return Ok(chartData);
        }


    }
}
