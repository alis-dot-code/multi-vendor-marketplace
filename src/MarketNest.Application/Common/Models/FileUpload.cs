namespace MarketNest.Application.Common.Models;

public class FileUpload
{
    public string FileName { get; set; } = string.Empty;
    public System.IO.Stream Content { get; set; } = Stream.Null;
}
