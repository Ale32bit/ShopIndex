namespace ShopIndexWebApp.Shared.ShopSync;

public class Location
{
    public float[] Coordinates { get; set; } = new float[3];
    public string? Description { get; set; }
    public string Dimension { get; set; } = "overworld";
}
