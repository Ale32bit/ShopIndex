namespace ShopIndexWebApp.Shared;

public class Shop
{
    public int Id { get; set; }
    public int ComputerId { get; set; }
    public string UID { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Owner { get; set; }
    public DateTime FirstSeen { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdate { get; set; }
    public string? Software { get; set; }
    public string? ActualLocation { get; set; }
    public string? Location { get; set; }
    public string? LocationDimension { get; set; }
    public string? LocationDescription { get; set; }
    public virtual ICollection<ShopItem> ShopItems { get; set; }
}
