using System.Text;
using System.Text.RegularExpressions;

namespace MarketNest.Application.Common.Utils;

public static class SlugUtil
{
    public static string GenerateSlug(string phrase)
    {
        var str = phrase.ToLowerInvariant();
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        str = Regex.Replace(str, @"\s+", " ").Trim();
        str = str.Substring(0, str.Length <= 200 ? str.Length : 200).Trim();
        str = Regex.Replace(str, @"\s", "-");
        return str;
    }
}
