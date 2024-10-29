using Bookshop_api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookshop_api.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options){}
        public DbSet<Book> Books { get; set; }
    }
}
