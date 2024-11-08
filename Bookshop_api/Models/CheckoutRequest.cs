namespace Bookshop_api.Models
{
    public class CheckoutRequest
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }

    public class CartItem
    {
        public string ProductName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
    }
}
