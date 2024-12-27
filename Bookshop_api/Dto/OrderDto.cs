namespace Bookshop_api.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime EstimatedDelivery { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
