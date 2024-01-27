using Microsoft.EntityFrameworkCore;
using ShopIndex.Models;

namespace ShopIndex.Data
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
