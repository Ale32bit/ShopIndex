using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopIndex.Data;
using ShopIndex.Models;
using System.Diagnostics;
using System.Reflection.Metadata;
using static ShopIndex.Models.ItemFilter;

namespace ShopIndex.Controllers;

public class HomeController : Controller
{
    public static int ItemsPerPage = 4 * 20;

    public readonly ItemFilter DefaultFilter = new()
    {
        Search = "",
        SellOnly = false,
        Sort = ItemFilter.FilterSort.Alphabetical,
    };
    private readonly ILogger<HomeController> _logger;
    private readonly DataContext _context;


    public HomeController(ILogger<HomeController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> Shops()
    {
        var shops = await _context.Shops.ToListAsync();
        return View(shops);
    }

    [HttpGet("[controller]/Shops/{shopId}")]
    public async Task<IActionResult> Shop(string shopId)
    {
        var shop = await _context.Shops.FirstOrDefaultAsync(q => q.UID == shopId);
        if (shop == null)
            return NotFound();
        var items = await _context.Items
            .Where(q=> q.ShopId == shop.Id)
            .ToListAsync();
        ViewData["Items"] = items;
        return View(shop);
    }

    public IActionResult Help()
    {
        return View();
    }
        

    [HttpGet("[controller]/GetItems")]
    public async Task<IActionResult> GetItems(int page = 1,
        string search = "",
        FilterSort sort = FilterSort.Alphabetical,
        string sellonly = "off")
    {
        var filter = new ItemFilter
        {
            Search = search,
            SellOnly = sellonly == "on",
            Sort = sort
        };

        var itemsQuery = _context.Items
            .Include(q => q.Shop)
            .Where(q => q.Name.ToLower().Contains(filter.Search)
            || q.Item.ToLower().Contains(filter.Search))
            .Where(q => filter.SellOnly ? q.ShopBuysItem : true)
            .AsEnumerable()
            .OrderBy(q => q.Stock == 0);

        itemsQuery = sort switch
        {
            FilterSort.Alphabetical => itemsQuery.ThenBy(q => q.Name),
            FilterSort.UpdateDate => itemsQuery,
            FilterSort.LowPrice => itemsQuery.ThenBy(q => q.GetPrice()),
            FilterSort.HighPrice => itemsQuery.ThenByDescending(q => q.GetPrice()),
            FilterSort.LowStock => itemsQuery.ThenBy(q => q.Stock),
            FilterSort.HighStock => itemsQuery.ThenByDescending(q => q.Stock),
            _ => itemsQuery,
        };


        var items = itemsQuery
            .Skip((page - 1) * ItemsPerPage)
            .Take(ItemsPerPage)
            .ToList();

        ViewData["Page"] = page;
        ViewData["Filter"] = filter;

        return PartialView("_ItemList", items);
    }

    [HttpGet("[controller]/GetItem/{id:int}")]
    public async Task<IActionResult> GetItem(int id)
    {
        var item = await _context.Items
            .Include(q => q.Shop)
            .FirstOrDefaultAsync(q => q.Id == id);

        return PartialView("_ItemDetails", item);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private IEnumerable<ShopItem> GetList(IEnumerable<ShopItem> items, ItemFilter filter)
    {
        var newList = items
            .Where(q =>
                q.Name.ToLower().Contains(filter.Search.ToLower())
                || q.Item.ToLower().Contains(filter.Search.ToLower()))
            .Where(q => filter.SellOnly ? q.ShopBuysItem : true)
            .OrderBy(q => q.Stock == 0);

        switch (filter.Sort)
        {
            case FilterSort.LowPrice:
                newList = newList.ThenBy(q => q.GetPrices().First().Price);
                break;
            case FilterSort.HighPrice:
                newList = newList.ThenByDescending(q => q.GetPrices().First().Price);
                break;
            case FilterSort.LowStock:
                newList = newList.ThenBy(q => q.Stock);
                break;
            case FilterSort.HighStock:
                newList = newList.ThenByDescending(q => q.Stock);
                break;
            case FilterSort.Alphabetical:
                newList = newList.ThenBy(q => q.Name);
                break;
        }

        return newList;
    }
}
