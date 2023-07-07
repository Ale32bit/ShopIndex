namespace ShopIndexWebApp.Shared;

public class SentryUpload
{
    public int ComputerId { get; set; }
    public ShopSync.ShopSync Data { get; set; }
    public string GetUID()
    {
        int computerId = Data.Info.ComputerID ?? ComputerId;
        return $"{computerId}:{Data.Info.MultiShop ?? 0}";
    }
}
