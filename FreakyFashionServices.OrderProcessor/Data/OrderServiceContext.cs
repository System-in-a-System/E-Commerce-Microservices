using FreakyFashionServices.OrderProcessor.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace FreakyFashionServices.OrderProcessor.Data
{
    public class OrderServiceContext : DbContext 
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=.;Database=OrderProcessing;Trusted_Connection=True");
        }
    }
}
