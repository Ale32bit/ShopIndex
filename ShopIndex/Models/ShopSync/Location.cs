namespace ShopIndex.Models.ShopSync;

public class Location
{
    public int[] Coordinates { get; set; } = new int[3];
    public string? Description { get; set; }
    public string Dimension { get; set; } = "overworld";
}
