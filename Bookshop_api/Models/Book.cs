using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookshop_api.Models
{
    public class Book : CommonProperties
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Image { get; set; } = string.Empty;
        public int? ISBN { get; set; } = null;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public virtual Category? Category { get; set; } = null!;
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual Author? Author { get; set; } = null!;
        public string Language { get; set; } = string.Empty;
        public double Price { get; set; } = 0.0;
    }
}
