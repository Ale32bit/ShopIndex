namespace ShopIndex.Models.ShopSync;

public class Info
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Owner { get; set; } = "Unknown";
    public int? MultiShop { get; set; }
    public Software? Software { get; set; }
    public Location? Location { get; set; }
}
