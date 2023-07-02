using MarleneCollectionXmlTool.Domain.Helpers;
using Microsoft.Extensions.Configuration;
using WinSCP;

namespace MarleneCollectionXmlTool.Domain.Services;

public interface IImageService
{
    Task<Dictionary<ulong, string>> AddImagesOnServer(List<ImagesWithNamesDto> imagesWithNames);
}

public record ImagesWithNamesDto (ulong ProductId, List<string> Urls, string FileName);
public record ImagesForProductUploadDto (ulong ProductId, byte[] Bytes, string FileName);

public class ImageService : IImageService
{
    private readonly HttpClient _httpClient;
    private readonly IWoocommerceRestApiService _woocommerceRestApiService;

    public ImageService(IConfiguration configuration, IWoocommerceRestApiService woocommerceRestApiService)
    {
        _httpClient = new HttpClient();
        _woocommerceRestApiService = woocommerceRestApiService;
    }

    public async Task<Dictionary<ulong, string>> AddImagesOnServer(List<ImagesWithNamesDto> imagesWithNames)
    {
        var images = new List<ImagesForProductUploadDto>();

        foreach (var imageWithName in imagesWithNames)
        {
            var productName = imageWithName.FileName.GenerateSlug();
            var productId = imageWithName.ProductId;

            var index = 1;
            foreach (var imageUrl in imageWithName.Urls)
            {
                var fileName = $"{productName}-{index}";

                var imageBytes = await _httpClient.GetByteArrayAsync(imageUrl);
                images.Add(new ImagesForProductUploadDto(productId, imageBytes, fileName));

                index++;
            }
        }

        var result = await PostImageToFtpServer(images);
        return result;
    }

    private async Task<Dictionary<ulong, string>> PostImageToFtpServer(List<ImagesForProductUploadDto> files)
    {
        var ftpUserName = "server681893_xml-service";
        var ftpPassword = "4yK7Mp1E^*n1";
        var ftpServer = "ftp.server681893.nazwa.pl";
        var destinationFolder = "wordpress/wpn_pierwszainstalacja/wp-content/uploads/2023/07";

        var sessionOptions = new SessionOptions
        {
            Protocol = Protocol.Sftp,
            HostName = ftpServer,
            UserName = ftpUserName,
            Password = ftpPassword,
            SshHostKeyFingerprint = "ecdsa-sha2-nistp256 256 nfa6DSkaZotWLQ1kHUBYzUmZ5sXY7OF7I3Soa7tpP5E",
        };

        var productIdUrlDict = new Dictionary<ulong, string>();
        
        using var session = new Session();

        try
        {
            session.Open(sessionOptions);

            foreach (var file in files)
            {
                var tempFilePath = Path.GetTempFileName();

                try
                {
                    var destinationPath = $"{destinationFolder}/{file.FileName}";
                    await File.WriteAllBytesAsync(tempFilePath, file.Bytes);
                    session.PutFiles(tempFilePath, destinationPath).Check();

                    productIdUrlDict.TryAdd(file.ProductId, destinationPath);

                    var data = new ProductDto
                    {
                        Images = new List<ProductDto.ImageDto>
                        {
                            new ProductDto.ImageDto { Src = destinationPath }
                        }
                    };

                    await _woocommerceRestApiService.UpdateProduct(file.ProductId, data);
                }
                finally
                {
                    File.Delete(tempFilePath);
                }
            }

            return productIdUrlDict;
        }
        catch (Exception e)
        {
            return productIdUrlDict;
        }
        finally
        {
            session.Close();
        }
    }

    internal class ProductDto
    {
        public List<ImageDto> Images { get; set; }

        internal class ImageDto
        {
            public string Src { get; set; }
        }
    }
}
