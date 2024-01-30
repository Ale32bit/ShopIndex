namespace ShopIndex.Models.ShopSync;

public class Item
{
    public string Name { get; set; } = "error";
    public string? NBT { get; set; } = null;
    public string DisplayName { get; set; }
    public string? Description { get; set; }

}
