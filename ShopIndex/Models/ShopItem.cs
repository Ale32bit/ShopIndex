using Microsoft.Extensions.ObjectPool;
using ShopIndex.Models.ShopSync;
using System.Security.Cryptography;
using System.Text;

namespace ShopIndex.Models;

public class ShopItem
{
    public int Id { get; set; }
    public int ShopId { get; set; }
    public string Name { get; set; }
    public string Item { get; set; }
    public string? NBT { get; set; }
    public string Hash => GetItemHash(Item, NBT, DynamicPrices, MadeOnDemand, ShopBuysItem);
    public int Stock { get; set; } = -1;
    public string PricesString { get; set; } = "";
    public bool DynamicPrices { get; set; } = false;
    public bool MadeOnDemand { get; set; } = false;
    public bool ShopBuysItem { get; set; } = false;
    public IEnumerable<ItemPrice> GetPrices() => GetPrices(PricesString);

    public virtual Shop Shop { get; set; }
    public static string GetItemHash(params object?[] pars)
    {
        var str = string.Join(':', pars);
        Console.WriteLine(str);
        return Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(str))
        );
    }

    public static IEnumerable<ItemPrice> GetPrices(string pricesString)
    {
        var prices = new List<ItemPrice>();
        var strPrices = pricesString.TrimEnd(';').Split(";");
        foreach(var strPrice in strPrices)
        {
            var price = strPrice.Split(' ');
            var value = double.Parse(price[0]);
            var currency = price[1].TrimEnd(';');
            prices.Add(new()
            {
                Price = value,
                Currency = currency,
            });
        }

        return prices;
    }

    public static string MakePricesString(IEnumerable<ShopItemPrice> prices)
    {
        var builder = new StringBuilder();

        foreach (var price in prices)
        {
            builder.AppendFormat("{0} {1};", price.Value, price.Currency);
        }

        return builder.ToString();
    }

}
