using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopIndex.Data;
using ShopIndex.Models;

namespace ShopIndex.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        public ShopController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("Shops")]
        public async Task<IEnumerable<Shop>> GetShops()
        {
            var shops = await _context.Shops.ToListAsync();

            return shops;
        }

        [HttpGet("Shops/Items")]
        public async Task<IEnumerable<ShopItem>> GetItems()
        {
            var items = await _context.Items.ToListAsync();

            return items;
        }

        [HttpGet("Shops/{uid}")]
        public async Task<Shop?> GetShop(string uid)
        {
            var shop = await _context.Shops.FirstOrDefaultAsync(q => q.UID == uid);

            return shop;
        }

        [HttpGet("Shops/{uid}/Items")]
        public async Task<IEnumerable<ShopItem>?> GetShopItems(string uid)
        {
            var shop = await _context.Shops.Include(q => q.ShopItems).FirstOrDefaultAsync(q => q.UID == uid);
            return shop?.ShopItems;
        }

        [HttpGet("Shops/{uid}/Items/{id:int}")]
        public async Task<ShopItem?> GetShopItem(string uid, int id)
        {
            var shop = await _context.Shops.Include(q => q.ShopItems).FirstOrDefaultAsync(q => q.UID == uid);
            return shop?.ShopItems.FirstOrDefault(q => q.Id == id);
        }

    }
}
