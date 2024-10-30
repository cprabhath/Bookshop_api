using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookshop_api.Models
{
    public class Order : CommonProperties
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int BookId { get; set; } = 0;
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; } = null!;
        public int CustomerId { get; set; } = 0;
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } = null!;
        public int Quantity { get; set; } = 0;
        public double TotalPrice { get; set; } = 0.0;
        public string Status { get; set; } = string.Empty;
    }
}
