using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Helpers;
using MarleneCollectionXmlTool.Domain.Queries.SyncProductStocksWithWholesales.Models;
using Microsoft.Extensions.Configuration;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net;
using System.Text.Json;
using System.Net.Cache;
using System.Text;
using WinSCP;

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

        await _dbContext.AddRangeAsync(wpPosts);
        await _dbContext.SaveChangesAsync();

        var wpMetas = new List<WpPostmetum>();

        foreach (var wpPost in wpPostsWithMetadata)
        {
            var postMetas = new List<WpPostmetum>
            {
                new WpPostmetum 
                {
                    PostId = wpPost.WpPost.Id,
                    MetaKey = "_wp_attached_file",
                    MetaValue = $"2023/07/{wpPost.WpPost.PostTitle}",
                },
                new WpPostmetum
                {
                    PostId = wpPost.WpPost.Id,
                    MetaKey = "_wp_attachment_metadata",
                    MetaValue = wpPost.Metadata,
                },
            };

            wpMetas.AddRange(postMetas);
        }

        await _dbContext.WpPostmeta.AddRangeAsync(wpMetas);
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
            var destinationFolder = "wordpress/wpn_pierwszainstalacja/wp-content/uploads/07";
            var credentials = new NetworkCredential(ftpUserName, ftpPassword);
            var productName = wpPost.PostTitle.GenerateSlug();

            var imageWpPosts = new List<WpPostWithMetadata>();

            var index = 1;
            foreach (var url in wpPost.ImageUrls)
            {
                var fileName = $"{productName}-{index}"; 
                
                var imageWpPost = await DownloadAndStoreImageAsync(url, fileName, destinationFolder, ftpServer, credentials);
                imageWpPosts.Add(imageWpPost);
                
                index++;
            }

            return imageWpPosts;
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
            var allImageSizes = allImageSizesWithMeta.imagesWithNames;
            var metadata = allImageSizesWithMeta.metadata;

            var extension = Path.GetExtension(imageUrl);
            var fileNameWithExtension = $"{fileName}{extension}";
            var destinationPath = $"ftp://{ftpServer}/{destinationFolder}/{fileNameWithExtension}";

            await PostImageToFtpServer(allImageSizes);

            var postName = fileNameWithExtension.Replace(".", "-");
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
                Guid = postGuid,
                MenuOrder = 0,
                PostType = "attachment",
                PostMimeType = $"image/{extension.Replace(".", string.Empty)}",
                CommentCount = 0
            };

            return new WpPostWithMetadata(post, metadata);
        }
        catch (Exception ex)
        {
            return new WpPostWithMetadata(null, null);
        }
    }

    private static async Task PostImageToFtpServer(List<(byte[] imageFile, string fileName)> files)
    {
        try
        {
            var ftpUserName = "server681893_xml-service";
            var ftpPassword = "4yK7Mp1E^*n1";
            var ftpServer = "ftp.server681893.nazwa.pl";
            var destinationFolder = "wordpress/wpn_pierwszainstalacja/wp-content/uploads/2023/07";

            // Set up session options
            var sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = ftpServer,
                UserName = ftpUserName,
                Password = ftpPassword,
                SshHostKeyFingerprint = "ecdsa-sha2-nistp256 256 nfa6DSkaZotWLQ1kHUBYzUmZ5sXY7OF7I3Soa7tpP5E",
            };

            using var session = new Session();
            session.Open(sessionOptions);

            foreach (var (imageFile, fileName) in files)
            {
                var tempFilePath = Path.GetTempFileName();

                try
                {
                    var destinationPath = $"wordpress/wpn_pierwszainstalacja/wp-content/uploads/2023/07/{fileName}";
                    await File.WriteAllBytesAsync(tempFilePath, imageFile);
                    session.PutFiles(tempFilePath, destinationPath).Check();
                }
                finally
                {
                    File.Delete(tempFilePath);
                }
            }
        }
        catch (Exception e)
        {

        }
    }

    private async Task<(List<(byte[] imageFile, string fileName)> imagesWithNames, string metadata)> GetAllImageSizes(string imageUrl, string fileName)
    {
        var mainImage = await _httpClient.GetByteArrayAsync(imageUrl);

        var width = 1000;
        var height = 1500;
        var extension = Path.GetExtension(imageUrl);
        var filePath = $"2023/07/{fileName}{extension}";
        var fileSize = mainImage.Length;
        var allImageSizes = new List<(byte[] imageFile, string fileName)>();
        var sizesMetadata = new Dictionary<string, ImageSizeMetadata>();

        allImageSizes.Add((mainImage, $"{fileName}{extension}"));

        var imageMedium = CropByteImage(mainImage, 200, 300);
        allImageSizes.Add((imageMedium, GetFileName(fileName, extension, 200, 300)));
        sizesMetadata.Add("medium", CreateImageSizeMetadata(fileName, extension, 200, 300, imageMedium.Length));

        var imageLarge = CropByteImage(mainImage, 683, 1024);
        allImageSizes.Add((imageLarge, GetFileName(fileName, extension, 683, 1024)));
        sizesMetadata.Add("large", CreateImageSizeMetadata(fileName, extension, 683, 1024, imageLarge.Length));

        var imageThumbnail = CropByteImage(mainImage, 150, 150);
        allImageSizes.Add((imageThumbnail, GetFileName(fileName, extension, 150, 150)));
        sizesMetadata.Add("thumbnail", CreateImageSizeMetadata(fileName, extension, 150, 150, imageThumbnail.Length));

        var medium_large = CropByteImage(mainImage, 768, 1152);
        allImageSizes.Add((medium_large, GetFileName(fileName, extension, 768, 1152)));
        sizesMetadata.Add("medium_large", CreateImageSizeMetadata(fileName, extension, 768, 1152, medium_large.Length));

        var postSliderThumbSize = CropByteImage(mainImage, 330, 190);
        allImageSizes.Add((postSliderThumbSize, GetFileName(fileName, extension, 330, 190)));
        sizesMetadata.Add("post-slider-thumb-size", CreateImageSizeMetadata(fileName, extension, 330, 190, postSliderThumbSize.Length));

        var postCategoryThumbSize = CropByteImage(mainImage, 330, 330);
        allImageSizes.Add((postCategoryThumbSize, GetFileName(fileName, extension, 330, 330)));
        sizesMetadata.Add("post-category-slider-size", CreateImageSizeMetadata(fileName, extension, 330, 330, postCategoryThumbSize.Length));

        var blossomShopSchema = CropByteImage(mainImage, 600, 60);
        allImageSizes.Add((blossomShopSchema, GetFileName(fileName, extension, 600, 60)));
        sizesMetadata.Add("blossom-shop-schema", CreateImageSizeMetadata(fileName, extension, 600, 60, blossomShopSchema.Length));

        var blossomShopBlog = CropByteImage(mainImage, 829, 623);
        allImageSizes.Add((blossomShopBlog, GetFileName(fileName, extension, 829, 623)));
        sizesMetadata.Add("blossom-shop-blog", CreateImageSizeMetadata(fileName, extension, 829, 623, blossomShopBlog.Length));

        var blossomShopBlogFull = CropByteImage(mainImage, 1000, 623);
        allImageSizes.Add((blossomShopBlogFull, GetFileName(fileName, extension, 1000, 623)));
        sizesMetadata.Add("blossom-shop-blog-full", CreateImageSizeMetadata(fileName, extension, 1000, 623, blossomShopBlogFull.Length));

        var blossomShopBlogList = CropByteImage(mainImage, 398, 297);
        allImageSizes.Add((blossomShopBlogList, GetFileName(fileName, extension, 398, 297)));
        sizesMetadata.Add("blossom-shop-blog-list", CreateImageSizeMetadata(fileName, extension, 398, 297, blossomShopBlogList.Length));

        var blossomShopSlider = CropByteImage(mainImage, 1000, 726);
        allImageSizes.Add((blossomShopSlider, GetFileName(fileName, extension, 1000, 726)));
        sizesMetadata.Add("blossom-shop-slider", CreateImageSizeMetadata(fileName, extension, 1000, 726, blossomShopSlider.Length));

        var blossomShopFeatured = CropByteImage(mainImage, 860, 860);
        allImageSizes.Add((blossomShopFeatured, GetFileName(fileName, extension, 860, 860)));
        sizesMetadata.Add("blossom-shop-featured", CreateImageSizeMetadata(fileName, extension, 860, 860, blossomShopFeatured.Length));

        var blossomShopRecent = CropByteImage(mainImage, 540, 810);
        allImageSizes.Add((blossomShopRecent, GetFileName(fileName, extension, 540, 810)));
        sizesMetadata.Add("blossom-shop-recent", CreateImageSizeMetadata(fileName, extension, 540, 810, blossomShopRecent.Length));

        var woocommerceThumbnail = CropByteImage(mainImage, 300, 300);
        allImageSizes.Add((woocommerceThumbnail, GetFileName(fileName, extension, 300, 300)));
        sizesMetadata.Add("woocommerce_thumbnail", CreateImageSizeMetadata(fileName, extension, 300, 300, woocommerceThumbnail.Length));

        var woocommerceSingle = CropByteImage(mainImage, 600, 900);
        allImageSizes.Add((woocommerceSingle, GetFileName(fileName, extension, 600, 900)));
        sizesMetadata.Add("woocommerce_single", CreateImageSizeMetadata(fileName, extension, 600, 900, woocommerceSingle.Length));

        var woocommerceGalleryThumbnail = CropByteImage(mainImage, 100, 100);
        allImageSizes.Add((woocommerceGalleryThumbnail, GetFileName(fileName, extension, 100, 100)));
        sizesMetadata.Add("woocommerce_gallery_thumbnail", CreateImageSizeMetadata(fileName, extension, 100, 100, woocommerceGalleryThumbnail.Length));

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

    private static ImageSizeMetadata CreateImageSizeMetadata(string fileName, string fileExtension, int width, int height, int fileSize) => new()
    {
        File = GetFileName(fileName, fileExtension, width, height),
        Width = width,
        Height = height,
        MimeType = $"image/{fileExtension.Replace(".", string.Empty)}",
        FileSize = fileSize
    };

    private static string GetFileName(string fileName, string fileExtension, int width, int height)
    {
        return $"{fileName}-{width}x{height}{fileExtension}";
    }

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
    }
}
