using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookshop_api.Models
{
    public class Customer : CommonProperties
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Image { get; set; } = "https://cdn-icons-png.flaticon.com/512/10141/10141012.png";
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;
        public string? ReadingGoals { get; set; } = string.Empty;
        public List<string>? FavoriteGenres { get; set; } = new List<string>();
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
