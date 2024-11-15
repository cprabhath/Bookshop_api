using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookshop_api.Models
{
    public class Order : CommonProperties
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? SessionId { get; set; }
        public string OrderId { get; set; } = string.Empty;

        public int CustomerId { get; set; } = 0;
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        public double TotalPrice { get; set; } = 0.0;
        public string Status { get; set; } = string.Empty;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
