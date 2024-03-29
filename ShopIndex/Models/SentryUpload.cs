﻿namespace ShopIndex.Models;

public class SentryUpload
{
    public int ComputerId { get; set; }
    public ShopSync.ShopSync Data { get; set; }
    public IEnumerable<double> Coordinates { get; set; } = Enumerable.Empty<double>();
    public Dimension? Dimension { get; set; }
    public string GetUID()
    {
        int computerId = Data.Info.ComputerID ?? ComputerId;
        return $"{computerId}:{Data.Info.MultiShop ?? 0}";
    }
}
