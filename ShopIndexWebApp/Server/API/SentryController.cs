using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopIndex.Data;
using ShopIndexWebApp.Shared;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShopIndex.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SentryController : ControllerBase
    {
        public static readonly Dictionary<string, SentryUpload> Dumps = new();

        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        public SentryController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("dump/{uid}")]
        public IActionResult DumpShop(string uid)
        {
            if (!Dumps.ContainsKey(uid))
                return NotFound();

            return new JsonResult(Dumps[uid]);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SentryUpload data)
        {
            var token = HttpContext.Request.Headers.Authorization.FirstOrDefault();
            if (token == null || _configuration["SentryToken"] != token)
            {
                return Forbid();
            }

            if (data.Data.Type != "ShopSync")
                return BadRequest();

            var sourceUID = data.GetUID();
            Dumps[sourceUID] = data;
            
            var shop = await _context.Shops.FirstOrDefaultAsync(q => q.UID == sourceUID);
            shop ??= new Shop()
            {
                UID = data.GetUID(),
                ComputerId = data.Data.Info.ComputerID ?? data.ComputerId,
                FirstSeen = DateTime.UtcNow,
            };

            if(shop.LastUpdate > DateTime.UtcNow.AddSeconds(-30))
            {
                return BadRequest("Rate limited");
            }

            var location = data.Data.Info.Location;
            shop.Name = data.Data.Info.Name;
            shop.Description = data.Data.Info.Description;
            shop.LastUpdate = DateTime.UtcNow;
            shop.Owner = data.Data.Info.Owner;

            if (location != null)
            {
                if(location.Coordinates.Length == 3)
                    shop.Location = $"{location.Coordinates?[0]} {location.Coordinates?[1]} {location.Coordinates?[2]}";
                shop.LocationDimension = location.Dimension;
                shop.LocationDescription = location.Description;
            }

            if (data.Data.Info.Software != null)
            {
                shop.Software = $"{data.Data.Info.Software.Name} {data.Data.Info.Software.Version}";
            }

            _context.Shops.Update(shop);
            await _context.SaveChangesAsync();

            var dbItems = await _context.Items.Where(q => q.ShopId == shop.Id).ToListAsync();
            var updatedItems = new List<string>();
            foreach (var item in data.Data.Items)
            {
                var hash = ShopItem.GetItemHash(item.Item.DisplayName, item.Item.Name, item.Item.NBT, item.DynamicPrice, item.MadeOnDemand, item.ShopBuysItem);

                var shopItem = dbItems.FirstOrDefault(q => q.Hash == hash);
                shopItem ??= new ShopItem
                {
                    Item = item.Item.Name,
                    NBT = item.Item.NBT,
                    ShopId = shop.Id,
                };

                shopItem.Name = item.Item.DisplayName;
                shopItem.Stock = item.Stock ?? -1;
                shopItem.PricesString = ShopItem.MakePricesString(item.Prices);
                shopItem.DynamicPrices = item.DynamicPrice;
                shopItem.MadeOnDemand = item.MadeOnDemand;
                shopItem.ShopBuysItem = item.ShopBuysItem;

                updatedItems.Add(shopItem.Hash);
                _context.Items.Update(shopItem);
            }

            // Remove items not in the new data
            var itemsToRemove = await _context.Items.Where(q => q.ShopId == shop.Id).ToListAsync();
            itemsToRemove.RemoveAll(q => updatedItems.Contains(q.Hash));
            _context.Items.RemoveRange(itemsToRemove);

            _context.SaveChangesFailed += _context_SaveChangesFailed;
            await _context.SaveChangesAsync();

            return Ok();
        }

        private void _context_SaveChangesFailed(object? sender, SaveChangesFailedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
