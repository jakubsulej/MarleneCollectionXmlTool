using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Helpers;
using MarleneCollectionXmlTool.Domain.Queries.SyncProductStocksWithWholesales.Models;
using Microsoft.Extensions.Configuration;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net;
using System.Text.Json;

namespace MarleneCollectionXmlTool.Domain.Services;

public interface IImageService
{
    Task<List<WpPostWithMetadata>> AddImagesForProduct(WpPostDto parentPostDto);
}

public record WpPostWithMetadata (WpPost WpPost, string Metadata);

public class ImageService : IImageService
{
    private readonly string _baseClientUrl;
    private readonly WoocommerceDbContext _dbContext;
    private readonly HttpClient _httpClient;

    public ImageService(IConfiguration configuration, WoocommerceDbContext dbContext)
    {
        _baseClientUrl = configuration.GetValue<string>("BaseClientUrl");
        _httpClient = new HttpClient();
        _dbContext = dbContext;
    }

    public async Task<List<WpPostWithMetadata>> AddImagesForProduct(WpPostDto parentPostDto)
    {
        var wpPostsWithMetadata = await UploadImagesOnServer(parentPostDto);
        var wpPosts = wpPostsWithMetadata.Select(x => x.WpPost).ToList();
        var wpMetas = wpPostsWithMetadata.Select(x => x.Metadata).ToList();

        await _dbContext.AddAsync(wpPosts);
        await _dbContext.AddAsync(wpMetas);
        await _dbContext.SaveChangesAsync();

        return wpPostsWithMetadata;
    }

    private async Task<List<WpPostWithMetadata>> UploadImagesOnServer(WpPostDto wpPost)
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

    private async Task<WpPostWithMetadata> DownloadAndStoreImageAsync(string imageUrl, string fileName, string destinationFolder, string ftpServer, NetworkCredential credentials)
    {
        try
        {
            var allImageSizesWithMeta = await GetAllImageSizes(imageUrl, fileName);
            var allImageSizes = allImageSizesWithMeta.images;
            var metadata = allImageSizesWithMeta.metadata;

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

            return new WpPostWithMetadata(post, metadata);
        }
        catch (Exception ex)
        {
            return new WpPostWithMetadata(null, null);
        }
    }

    private async Task<(List<byte[]> images, string metadata)> GetAllImageSizes(string imageUrl, string fileName)
    {
        var mainImage = await _httpClient.GetByteArrayAsync(imageUrl);

        int width = 1000;
        int height = 1500;
        string filePath = $"2023/07/{fileName}.jpeg";
        int fileSize = 267178;
        var allImageSizes = new List<byte[]>();
        var sizesMetadata = new Dictionary<string, ImageSizeMetadata>();

        allImageSizes.Add(mainImage);

        var imageMedium = CropByteImage(mainImage, 200, 300);
        allImageSizes.Add(imageMedium);
        sizesMetadata.Add("medium", CreateImageSizeMetadata(fileName, 200, 300, imageMedium.Length));

        var imageLarge = CropByteImage(mainImage, 683, 1024);
        allImageSizes.Add(imageLarge);
        sizesMetadata.Add("large", CreateImageSizeMetadata(fileName, 683, 1024, imageMedium.Length));

        var imageThumbnail = CropByteImage(mainImage, 150, 150);
        allImageSizes.Add(imageThumbnail);
        sizesMetadata.Add("thumbnail", CreateImageSizeMetadata(fileName, 683, 1024, imageMedium.Length));

        var medium_large = CropByteImage(mainImage, 768, 1152);
        allImageSizes.Add(medium_large);
        sizesMetadata.Add("medium_large", CreateImageSizeMetadata(fileName, 683, 1024, imageMedium.Length));

        var postSliderThumbSize = CropByteImage(mainImage, 330, 190);
        allImageSizes.Add(postSliderThumbSize);
        sizesMetadata.Add("post-slider-thumb-size", CreateImageSizeMetadata(fileName, 683, 1024, imageMedium.Length));

        var postCategoryThumbSize = CropByteImage(mainImage, 330, 330);
        allImageSizes.Add(postCategoryThumbSize);
        sizesMetadata.Add("post-category-slider-size", CreateImageSizeMetadata(fileName, 683, 1024, imageMedium.Length));

        var blossomShopSchema = CropByteImage(mainImage, 600, 60);
        allImageSizes.Add(blossomShopSchema);
        sizesMetadata.Add("blossom-shop-schema", CreateImageSizeMetadata(fileName, 683, 1024, imageMedium.Length));

        var blossomShopBlog = CropByteImage(mainImage, 829, 623);
        allImageSizes.Add(blossomShopBlog);
        sizesMetadata.Add("blossom-shop-blog", CreateImageSizeMetadata(fileName, 683, 1024, imageMedium.Length));

        var blossomShopBlogFull = CropByteImage(mainImage, 1000, 623);
        allImageSizes.Add(blossomShopBlogFull);
        sizesMetadata.Add("blossom-shop-blog-full", CreateImageSizeMetadata(fileName, 683, 1024, imageMedium.Length));

        var blossomShopBlogList = CropByteImage(mainImage, 398, 297);
        allImageSizes.Add(blossomShopBlogList);
        sizesMetadata.Add("blossom-shop-blog-list", CreateImageSizeMetadata(fileName, 683, 1024, imageMedium.Length));

        var blossomShopSlider = CropByteImage(mainImage, 1000, 726);
        allImageSizes.Add(blossomShopSlider);
        sizesMetadata.Add("blossom-shop-slider", CreateImageSizeMetadata(fileName, 683, 1024, imageMedium.Length));

        var blossomShopFeatured = CropByteImage(mainImage, 860, 860);
        allImageSizes.Add(blossomShopFeatured);
        sizesMetadata.Add("blossom-shop-featured", CreateImageSizeMetadata(fileName, 683, 1024, imageMedium.Length));

        var blossomShopRecent = CropByteImage(mainImage, 540, 810);
        allImageSizes.Add(blossomShopRecent);
        sizesMetadata.Add("blossom-shop-recent", CreateImageSizeMetadata(fileName, 683, 1024, imageMedium.Length));

        var woocommerceThumbnail = CropByteImage(mainImage, 300, 300);
        allImageSizes.Add(woocommerceThumbnail);
        sizesMetadata.Add("woocommerce_thumbnail", CreateImageSizeMetadata(fileName, 683, 1024, imageMedium.Length));

        var woocommerceSingle = CropByteImage(mainImage, 600, 900);
        allImageSizes.Add(woocommerceSingle);
        sizesMetadata.Add("woocommerce_single", CreateImageSizeMetadata(fileName, 683, 1024, imageMedium.Length));

        var woocommerceGalleryThumbnail = CropByteImage(mainImage, 100, 100);
        allImageSizes.Add(woocommerceGalleryThumbnail);
        sizesMetadata.Add("woocommerce_gallery_thumbnail", CreateImageSizeMetadata(fileName, 683, 1024, imageMedium.Length));

        var metadata = GenerateImageMetadata(width, height, filePath, fileSize, sizesMetadata);

        return (allImageSizes, metadata);
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

    private static ImageSizeMetadata CreateImageSizeMetadata(string fileName, int width, int height, int fileSize) => new()
    {
        File = $"{fileName}-{width}x{height}.jpeg",
        Width = width,
        Height = height,
        MimeType = "image/jpeg",
        FileSize = fileSize
    };

    private static string GenerateImageMetadata(int width, int height, string filePath, int fileSize, Dictionary<string, ImageSizeMetadata> sizes)
    {
        var imageMeta = new Dictionary<string, string>
        {
            { "aperture", "0" },
            { "credit", "" },
            { "camera", "" },
            { "caption", "" },
            { "created_timestamp", "0" },
            { "copyright", "" },
            { "focal_length", "0" },
            { "iso", "0" },
            { "shutter_speed", "0" },
            { "title", "" },
            { "orientation", "0" },
            { "keywords", "" }
        };

        var metadata = new ImageMetadata
        {
            Width = width,
            Height = height,
            File = filePath,
            FileSize = fileSize,
            Sizes = sizes,
            ImageMeta = imageMeta
        };

        return JsonSerializer.Serialize(metadata, new JsonSerializerOptions { WriteIndented = true });
    }

    private class ImageMetadata
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string File { get; set; }
        public int FileSize { get; set; }
        public Dictionary<string, ImageSizeMetadata> Sizes { get; set; }
        public Dictionary<string, string> ImageMeta { get; set; }
    }

    private class ImageSizeMetadata
    {
        public string File { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string MimeType { get; set; }
        public int FileSize { get; set; }
        public bool? Uncropped { get; set; }
    }
}
