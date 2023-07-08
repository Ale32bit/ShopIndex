using Microsoft.EntityFrameworkCore;
using ShopIndexWebApp.Shared;

namespace ShopIndexWebApp.Server.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ShopItem> Items { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
    }
}
