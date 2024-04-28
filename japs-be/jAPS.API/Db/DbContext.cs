using jAPS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace jAPS.API.Db
{
    public class JapsDbContext : DbContext
    {
        public JapsDbContext(DbContextOptions<JapsDbContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrdersItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .HasKey(t => new { t.TransactionId, t.BasketId });
        }
    }
}
