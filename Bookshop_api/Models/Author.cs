namespace Bookshop_api.Models
{
    public class Author : CommonProperties
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
