using jAPS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace jAPS.API.Db
{
    public class JapsDbContext : DbContext
    {
        public JapsDbContext(DbContextOptions<JapsDbContext> options) : base(options) { }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrdersItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .HasKey(t => new { t.TransactionId, t.BasketId });
        }
    }
}




