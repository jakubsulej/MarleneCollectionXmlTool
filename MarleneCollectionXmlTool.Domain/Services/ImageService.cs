using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Helpers;
using MarleneCollectionXmlTool.Domain.Queries.SyncProductStocksWithWholesales.Models;
using Microsoft.Extensions.Configuration;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net;
using System.Security.Policy;

namespace MarleneCollectionXmlTool.Domain.Services;

public interface IImageService
{
    Task AddImagesForProduct(WpPostDto parentPostDto);
    Task<List<WpPost>> UploadImagesOnServer(WpPostDto wpPost);
}

public class ImageService : IImageService
{
    private readonly string _baseClientUrl;
    private readonly WoocommerceDbContext _dbContext;
    private HttpClient _httpClient;

    public ImageService(IConfiguration configuration, WoocommerceDbContext dbContext)
    {
        _baseClientUrl = configuration.GetValue<string>("BaseClientUrl");
        _dbContext = dbContext;
        _httpClient = new HttpClient();
    }

    public async Task AddImagesForProduct(WpPostDto parentPostDto)
    {
        var wpPosts = await UploadImagesOnServer(parentPostDto);



        //await _dbContext.WpPosts.AddRangeAsync(wpPosts);
        //await _dbContext.SaveChangesAsync();
    }

    public async Task<List<WpPost>> UploadImagesOnServer(WpPostDto wpPost)
    {
        try
        {
            var ftpUserName = "server681893_xml-service";
            var ftpPassword = "4yK7Mp1E^*n1";
            var ftpServer = "ftp.server681893.nazwa.pl";
            var destinationFolder = "/wordpress/wpn_pierwszainstalacja/wp-content/uploads/07";
            var credentials = new NetworkCredential(ftpUserName, ftpPassword);
            var fileName = wpPost.PostTitle.GenerateSlug();

            var imageWpPosts = await Task.WhenAll(wpPost.ImageUrls
                .Select(url => DownloadAndStoreImageAsync(url, fileName, destinationFolder, ftpServer, credentials)));

            return imageWpPosts.ToList();
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private async Task<WpPost> DownloadAndStoreImageAsync(string imageUrl, string fileName, string destinationFolder, string ftpServer, NetworkCredential credentials)
    {
        try
        {
            var allImageSizes = await GetAllImageSizes(imageUrl, fileName);

            var extension = Path.GetExtension(imageUrl);
            var fileNameWithExtension = $"{fileName}{extension}";
            var destinationPath = $"ftp://{ftpServer}/{destinationFolder}/{fileNameWithExtension}";

            using (var ftpClient = new WebClient())
            {
                ftpClient.Credentials = credentials;

                foreach (var file in allImageSizes)
                {
                    await ftpClient.UploadDataTaskAsync(destinationPath, file);
                }
            }

            var postName = fileNameWithExtension.GenerateSlug();
            var postGuid = $"{_baseClientUrl}/{postName}";

            var post = new WpPost
            {
                PostAuthor = 2,
                PostDate = DateTime.Now,
                PostDateGmt = DateTime.UtcNow,
                PostContent = string.Empty,
                PostTitle = fileNameWithExtension,
                PostExcerpt = string.Empty,
                PostStatus = "inherit",
                CommentStatus = "open",
                PingStatus = "closed",
                PostPassword = string.Empty,
                PostName = postName,
                ToPing = string.Empty,
                Pinged = string.Empty,
                PostModified = DateTime.Now,
                PostModifiedGmt = DateTime.UtcNow,
                PostContentFiltered = string.Empty,
                PostParent = 0,
                Guid = destinationPath,
                MenuOrder = 0,
                PostType = "attachment",
                PostMimeType = $"image/{extension}",
                CommentCount = 0
            };

            return post;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private async Task<List<byte[]>> GetAllImageSizes(string imageUrl, string fileName)
    {
        var mainImage = await _httpClient.GetByteArrayAsync(imageUrl);

        var allImageSizes = new List<byte[]>();

        var imageMedium = CropByteImage(mainImage, 200, 300);
        //var imageMediumMeta = 

        var imageLarge = CropByteImage(mainImage, 683, 1024);
        var imageThumbnail = CropByteImage(mainImage, 150, 150);
        var medium_large = CropByteImage(mainImage, 768, 1152);
        var postSliderThumbSize = CropByteImage(mainImage, 330, 190);
        var postCategoryThumbSize = CropByteImage(mainImage, 330, 330);
        var blossomShopSchema = CropByteImage(mainImage, 600, 60);
        var blossomShopBlog = CropByteImage(mainImage, 829, 623);
        var blossomShopBlogFull = CropByteImage(mainImage, 1000, 623);
        var blossomShopBlogList = CropByteImage(mainImage, 398, 297);
        var blossomShopSlider = CropByteImage(mainImage, 1000, 726);
        var blossomShopFeatured = CropByteImage(mainImage, 860, 860);
        var blossomShopRecent = CropByteImage(mainImage, 540, 810);
        var woocommerceThumbnail = CropByteImage(mainImage, 300, 300);
        var woocommerceSingle = CropByteImage(mainImage, 600, 900);
        var woocommerceGalleryThumbnail = CropByteImage(mainImage, 100, 100);

        return allImageSizes;
    }

    private static byte[] CropByteImage(byte[] imageBytes, int width, int height)
    {
        using var inputStream = new MemoryStream(imageBytes);
        using var outputStream = new MemoryStream();

        var image = Image.FromStream(inputStream);
        var croppedImage = CropImage(image, width, height);
        croppedImage.Save(outputStream, ImageFormat.Jpeg);

        var result = outputStream.ToArray();
        return result;
    }

    private static Image CropImage(Image image, int width, int height)
    {
        var croppedImage = new Bitmap(width, height);
        using (var graphics = Graphics.FromImage(croppedImage))
        {
            graphics.DrawImage(image, 0, 0, width, height);
        }
        return croppedImage;
    }
}
