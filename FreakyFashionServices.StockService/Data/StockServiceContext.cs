using FreakyFashionServices.StockService.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace FreakyFashionServices.StockService.Data
{
    public class StockServiceContext : DbContext
    {
        public DbSet<StockLevel> StockLevel { get; set; }

        public StockServiceContext(DbContextOptions<StockServiceContext> options)
            : base(options)
        {

        }
    }
}
