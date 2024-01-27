using Humanizer;
using System.Globalization;

namespace ShopIndex;

public static class DateTimeExtensions
{
    public static string ToRelative(this DateTime datetime, CultureInfo? culture = null)
    {
        try
        {
            return datetime.Humanize(culture: culture, utcDate: true);
        }
        catch
        {
            return datetime.Humanize(culture: CultureInfo.InvariantCulture, utcDate: true);
        }
    }

    public static string ToRelative(this DateTime? datetime, CultureInfo? culture = null)
    {
        try
        {
            return datetime.Humanize(culture: culture, utcDate: true);
        }
        catch
        {
            return datetime.Humanize(culture: CultureInfo.InvariantCulture, utcDate: true);
        }
    }
}