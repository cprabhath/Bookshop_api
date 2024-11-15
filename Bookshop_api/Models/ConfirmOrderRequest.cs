namespace Bookshop_api.Models
{
    public class ConfirmOrderRequest
    {
        public string SessionId { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public List<CartItems> CartItems { get; set; } = new List<CartItems>();
    }

    public class CartItems
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; } 
    }

}
