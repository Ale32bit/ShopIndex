namespace ShopIndexWebApp.Shared.ShopSync;

/// <summary>
/// Broadcast data sent by shops.
/// Shops broadcast a Lua table on channel 9773, the reply channel is the computer ID.
/// </summary>
public class ShopSync
{
    public string Type { get; set; }
    public Info Info { get; set; }
    public IEnumerable<ShopItem> Items { get; set; } = Enumerable.Empty<ShopItem>();
}
