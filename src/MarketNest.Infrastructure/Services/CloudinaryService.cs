using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MarketNest.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MarketNest.Infrastructure.Services;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration config)
    {
        var account = new Account(
            config["CloudinarySettings:CloudName"],
            config["CloudinarySettings:ApiKey"],
            config["CloudinarySettings:ApiSecret"]
        );
        _cloudinary = new Cloudinary(account);
    }

    public async Task<(string Url, string PublicId)> UploadImageAsync(Stream file, string folder)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription("image", file),
            Folder = $"marketnest/{folder}",
            Transformation = new Transformation()
                .Width(1200)
                .Crop("limit")
                .Quality("auto")
                .FetchFormat("auto")
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        return (result.SecureUrl.ToString(), result.PublicId);
    }

    public async Task<bool> DeleteImageAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);
        return result.Result == "ok";
    }
}
