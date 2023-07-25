using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.Domain.Helpers.Extensions;
using MarleneCollectionXmlTool.Domain.Services.ClientSevices;
using Microsoft.Extensions.Configuration;
namespace MarleneCollectionXmlTool.Domain.Services.ProductUpdaters;

public interface IImageService
{
    Task DownloadImagesOnLocalMachine(List<ImagesWithNamesDto> imagesWithNames);
}

public record ImagesWithNamesDto(ulong ProductId, List<string> Urls, string FileName, string Sku);
public record ImagesForProductUploadDto(ulong ProductId, byte[] Bytes, string FileName, string ImageName, string Sku);

public class ImageService : IImageService
{
    private readonly HttpClient _httpClient;

    public ImageService()
    {
        _httpClient = new HttpClient();
    }


    public async Task DownloadImagesOnLocalMachine(List<ImagesWithNamesDto> imagesWithNames)
    {
        var images = new List<ImagesForProductUploadDto>();

        foreach (var imageWithName in imagesWithNames)
        {
            var productName = imageWithName.FileName.GenerateSlug();
            var productId = imageWithName.ProductId;

            var index = 1;
            foreach (var imageUrl in imageWithName.Urls)
            {
                var extension = Path.GetExtension(imageUrl);
                var fileName = $"{productName}-{index}{extension}";

                var imageBytes = await _httpClient.GetByteArrayAsync(imageUrl);
                images.Add(new ImagesForProductUploadDto(productId, imageBytes, fileName, imageWithName.FileName, imageWithName.Sku));

                index++;
            }
        }

        await DownloadImagesToLocalMachine(images);
    }

    private static async Task DownloadImagesToLocalMachine(List<ImagesForProductUploadDto> files)
    {
        try
        {
            foreach (var file in files)
            {
                var baseDirectory = @"C:\MarleneCollectionImages";
                var folderDirectory = Path.Combine(baseDirectory, file.Sku);

                Directory.CreateDirectory(folderDirectory);

                var productDirectory = Path.Combine(folderDirectory, file.FileName);
                await File.WriteAllBytesAsync(productDirectory, file.Bytes);
            }
        }
        catch
        {
            throw;
        }
    }
}
