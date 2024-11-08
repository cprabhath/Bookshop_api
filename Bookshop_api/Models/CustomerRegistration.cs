namespace Bookshop_api.Models
{
    public class CustomerRegistration
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string ReadingGoals { get; set; }
        public List<string> FavoriteGenres { get; set; } = new List<string>();

    }
}
