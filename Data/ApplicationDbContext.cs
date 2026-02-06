using BlackBoxInc.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlackBoxInc.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<CartItem>()
        //        .HasOne(ci => ci.User)
        //        .WithMany(c => c.Items)
        //        .HasForeignKey(ci => ci.UserId)
        //        .OnDelete(DeleteBehavior.Cascade);
        //}


        public DbSet<Products> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

    }
}
