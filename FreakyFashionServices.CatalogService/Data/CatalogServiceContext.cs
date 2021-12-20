using FreakyFashionServices.CatalogService.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace FreakyFashionServices.CatalogService.Data
{
    public class CatalogServiceContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public CatalogServiceContext(DbContextOptions<CatalogServiceContext> options)
            : base(options)
        {

        }
    }
}
