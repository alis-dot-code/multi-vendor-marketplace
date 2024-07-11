namespace MarketNest.Application.Common.Interfaces;

public interface ICloudinaryService
{
    Task<(string Url, string PublicId)> UploadImageAsync(Stream file, string folder);
    Task<bool> DeleteImageAsync(string publicId);
}
