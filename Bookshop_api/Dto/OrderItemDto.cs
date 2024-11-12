namespace Bookshop_api.Dto
{
    public class OrderItemDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
    }
}
