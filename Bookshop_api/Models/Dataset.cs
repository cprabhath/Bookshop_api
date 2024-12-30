namespace Bookshop_api.Models
{
    public class Dataset
    {
        public List<double> Data { get; set; } = new List<double>();
        public List<string> BackgroundColor { get; set; } = new List<string>();
        public int HoverOffset { get; set; }
    }
}
