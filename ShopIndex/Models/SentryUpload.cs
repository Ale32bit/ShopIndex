namespace ShopIndex.Models;

public class SentryUpload
{
    public int ComputerId { get; set; }
    public ShopSync.ShopSync Data { get; set; }
    public string GetUID()
    {
        return $"{ComputerId}:{Data.Info.MultiShop ?? 0}";
    }
}
