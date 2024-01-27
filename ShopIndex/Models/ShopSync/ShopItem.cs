namespace ShopIndex.Models.ShopSync;

public class ShopItem
{
    public IEnumerable<ShopItemPrice> Prices { get; set; } = Enumerable.Empty<ShopItemPrice>();
    public Item Item { get; set; }
    public bool DynamicPrice { get; set; } = false;
    public int? Stock { get; set; }
    public bool MadeOnDemand { get; set; } = false;
    public bool RequiresInteraction { get; set; } = false;
    public bool ShopBuysItem { get; set; } = false;
    public bool NoLimit { get; set; } = false;
}
