using System.Data;

namespace Bookshop_api.Models
{
    public class BestSellingBookReport
    {
        public string BookTitle { get; set; }
        public int QuantitySold { get; set; }
        public double TotalRevenue { get; set; }
    }
}
