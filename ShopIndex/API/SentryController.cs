﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopIndex.Data;
using ShopIndex.Models;
using ShopIndex.Models.ShopSync;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShopIndex.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SentryController : ControllerBase
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {

        };

        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        public SentryController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello");
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

            var shop = await _context.Shops.FirstOrDefaultAsync(q => q.UID == data.GetUID());
            shop ??= new Shop()
            {
                UID = data.GetUID(),
                ComputerId = data.ComputerId,
                FirstSeen = DateTime.UtcNow,
            };

            var location = data.Data.Info.Location;
            shop.Name = data.Data.Info.Name;
            shop.Description = data.Data.Info.Description;
            shop.LastUpdate = DateTime.UtcNow;
            shop.Owner = data.Data.Info.Owner;

            if (location != null)
            {
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
                var hash = Models.ShopItem.GetItemHash(item.Item.Name, item.Item.NBT);

                var shopItem = dbItems.FirstOrDefault(q => q.Hash == hash);
                shopItem ??= new Models.ShopItem
                {
                    Item = item.Item.Name,
                    NBT = item.Item.NBT,
                    ShopId = shop.Id,
                };

                shopItem.Name = item.Item.DisplayName;
                shopItem.Stock = item.Stock ?? -1;
                shopItem.PricesString = Models.ShopItem.MakePricesString(item.Prices);

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