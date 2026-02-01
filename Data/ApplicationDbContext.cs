using BlackBoxInc.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlackBoxInc.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }

        public DbSet<Products> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }

    }
}
