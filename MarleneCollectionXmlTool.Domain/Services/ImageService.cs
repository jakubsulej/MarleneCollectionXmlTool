using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.Domain.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WinSCP;

namespace MarleneCollectionXmlTool.Domain.Services;

public interface IImageService
{
    Task<Dictionary<ulong, string>> AddImagesOnServer(List<ImagesWithNamesDto> imagesWithNames);
    Task DownloadImagesOnLocalMachine(List<ImagesWithNamesDto> imagesWithNames);
}

public record ImagesWithNamesDto (ulong ProductId, List<string> Urls, string FileName, string Sku);
public record ImagesForProductUploadDto (ulong ProductId, byte[] Bytes, string FileName, string ImageName, string Sku);

public class ImageService : IImageService
{
    private readonly HttpClient _httpClient;
    private readonly IWoocommerceRestApiService _woocommerceRestApiService;
    private readonly WoocommerceDbContext _dbContext;
    private readonly string _ftpUserName;
    private readonly string _ftpPassword;
    private readonly string _clientBaseUrl;
    private readonly string _ftpServer;
    private readonly string _ftpFolder;
    private readonly string _destinationFolder;
    private readonly string _sshHostFingerprint;

    public ImageService(
        IConfiguration configuration, 
        IWoocommerceRestApiService woocommerceRestApiService,
        WoocommerceDbContext dbContext)
    {
        _httpClient = new HttpClient();
        _woocommerceRestApiService = woocommerceRestApiService;
        _dbContext = dbContext;
        _ftpUserName = configuration.GetValue<string>("FtpUserName");
        _ftpPassword = configuration.GetValue<string>("FtpPassword");
        _clientBaseUrl = configuration.GetValue<string>("BaseClientUrl");
        _ftpServer = configuration.GetValue<string>("FtpServer");
        _ftpFolder = configuration.GetValue<string>("FtpFolder");
        _destinationFolder = configuration.GetValue<string>("DestinationFolder");
        _sshHostFingerprint = configuration.GetValue<string>("SshHostFingerpring");
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
                images.Add(new ImagesForProductUploadDto(productId, imageBytes, fileName, imageWithName.FileName, imageWithName.Sku));

                index++;
            }
        }

        var result = await PostImageToFtpServer(images);
        return result;
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
        catch (Exception e)
        {

        }
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
        
        var lastPostId = _dbContext.WpPosts
            .OrderByDescending(x => x.Id)
            .Select(x => x.Id)
            .FirstOrDefault();

        using var session = new Session();

        try
        {
            session.Open(sessionOptions);

            var productFiles = files.GroupBy(x => x.ProductId).Select(x => x.ToList()).ToList();

            foreach (var productFile in productFiles)
            {
                var productId = productFile.First().ProductId;
                var data = new ProductDto();

                foreach (var file in productFile)
                {
                    var tempFilePath = Path.GetTempFileName();

                    try
                    {
                        var destinationPath = $"wordpress/wpn_pierwszainstalacja/wp-content/uploads/2023/07/{file.FileName}";
                        await File.WriteAllBytesAsync(tempFilePath, file.Bytes);
                        
                        session.PutFiles(tempFilePath, destinationPath).Check();
                        var imageName = Path.GetFileNameWithoutExtension(file.FileName);

                        productIdUrlDict.TryAdd(file.ProductId, destinationPath);
                        var absolutePath = $"{_clientBaseUrl}/{_destinationFolder}/{file.FileName}";

                        data.Images.Add(new ProductDto.ImageDto { Id = lastPostId++ });
                        data.Images.Add(new ProductDto.ImageDto
                        {
                            Src = absolutePath,
                            Name = file.ImageName,
                            Alt = file.ImageName
                        });
                    }
                    finally
                    {
                        File.Delete(tempFilePath);
                    }
                }

                var json = JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings 
                { 
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });
                await _woocommerceRestApiService.UpdateProduct(productId, json);
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
        [JsonProperty(PropertyName = "images")]
        public List<ImageDto> Images { get; set; } = new List<ImageDto>();

        internal class ImageDto
        {
            [JsonProperty(PropertyName = "id")]
            public ulong Id { get; set; }

            [JsonProperty(PropertyName = "src")]
            public string Src { get; set; }

            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "alt")]
            public string Alt { get; set; }
        }
    }
}
