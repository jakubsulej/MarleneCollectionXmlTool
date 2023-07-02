using MarleneCollectionXmlTool.Domain.Helpers;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    private readonly string _ftpUserName;
    private readonly string _ftpPassword;
    private readonly string _clientBaseUrl;
    private readonly string _ftpServer;
    private readonly string _ftpFolder;
    private readonly string _destinationFolder;
    private readonly string _sshHostFingerprint;

    public ImageService(IConfiguration configuration, IWoocommerceRestApiService woocommerceRestApiService)
    {
        _httpClient = new HttpClient();
        _woocommerceRestApiService = woocommerceRestApiService;
        _ftpUserName = configuration.GetValue<string>("FtpUserName");
        _ftpPassword = configuration.GetValue<string>("FtpPassword");
        _clientBaseUrl = configuration.GetValue<string>("BaseClientUrl");
        _ftpServer = configuration.GetValue<string>("FtpServer");
        _ftpFolder = configuration.GetValue<string>("FtpFolder");
        _destinationFolder = configuration.GetValue<string>("DestinationFolder");
        _sshHostFingerprint = configuration.GetValue<string>("SshHostFingerpring");
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
                var extension = Path.GetExtension(imageUrl);
                var fileName = $"{productName}-{index}{extension}";

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
        var sessionOptions = new SessionOptions
        {
            Protocol = Protocol.Sftp,
            HostName = _ftpServer,
            UserName = _ftpUserName,
            Password = _ftpPassword,
            SshHostKeyFingerprint = _sshHostFingerprint,
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
                    var destinationPath = $"wordpress/wpn_pierwszainstalacja/wp-content/uploads/2023/07/{file.FileName}";
                    await File.WriteAllBytesAsync(tempFilePath, file.Bytes);
                    session.PutFiles(tempFilePath, destinationPath).Check();

                    productIdUrlDict.TryAdd(file.ProductId, destinationPath);
                    var absolutePath = $"{_clientBaseUrl}/{_destinationFolder}/{file.FileName}";

                    var data = new ProductDto
                    {
                        Images = new List<ProductDto.ImageDto>
                        {
                            new ProductDto.ImageDto { Src = absolutePath }
                        }
                    };

                    var dataString = JsonSerializer.Serialize(data);

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
