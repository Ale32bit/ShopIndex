using System.Text;
using System.Web;
using static ShopIndex.Models.ItemFilter;

namespace ShopIndex.Models;

public class ItemFilter
{
    public enum FilterSort
    {
        Alphabetical,
        UpdateDate,
        LowPrice,
        HighPrice,
        LowStock,
        HighStock,
    }

    public string Search { get; set; } = "";
    public FilterSort Sort { get; set; }
    public bool SellOnly { get; set; } = false;

    public string Unpack()
    {
        var builder = new StringBuilder();

        builder.Append("search=");
        builder.Append(HttpUtility.UrlEncode(Search));
        builder.Append("&sort=");
        builder.Append((int)Sort);
        builder.Append("&sellonly=");
        builder.Append(SellOnly ? "on" : "off");

        return builder.ToString();
    }

}

public static class FilterSortExtensions
{
    private static readonly Dictionary<FilterSort, string> _locale = new()
    {
        [FilterSort.Alphabetical] = "Alphabetical",
        [FilterSort.UpdateDate] = "Recently updated",
        [FilterSort.LowPrice] = "Lowest price",
        [FilterSort.HighPrice] = "Highest price",
        [FilterSort.LowStock] = "Lowest stock",
        [FilterSort.HighStock] = "Highest stock",
    };
    public static string GetName(this FilterSort sort)
    {
        return _locale[sort];
    }
}

