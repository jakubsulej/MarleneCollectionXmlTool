using FluentResults;
using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.Domain.Queries.SyncProductStocksWithWholesales.Models;
using MarleneCollectionXmlTool.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace MarleneCollectionXmlTool.Domain.Queries.UploadProductImages;

public class UploadProductImagesRequestHandler : IRequestHandler<UploadProductImagesRequest, Result<UploadProductImagesResponse>>
{
    private readonly IGetXmlDocumentFromWholesalerService _wholesalerService;
    private readonly IWoocommerceRestApiService _woocommerceRestApiService;
    private readonly IImageService _imageService;
    private readonly WoocommerceDbContext _dbContext;

    public UploadProductImagesRequestHandler(
        IGetXmlDocumentFromWholesalerService wholesalerService,
        IWoocommerceRestApiService woocommerceRestApiService,
        IImageService imageService,
        WoocommerceDbContext dbContext)
    {
        _wholesalerService = wholesalerService;
        _woocommerceRestApiService = woocommerceRestApiService;
        _imageService = imageService;
        _dbContext = dbContext;
    }

    public async Task<Result<UploadProductImagesResponse>> Handle(UploadProductImagesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var xmlDoc = await _wholesalerService.GetXmlDocumentNestedVariantsXmlUrl(cancellationToken);
            var xmlProducts = xmlDoc.GetElementsByTagName("produkt");

            var parentSkuImageDict = new Dictionary<string, (string postTitle, List<string> imageUrls)>();

            foreach (XmlNode xmlProduct in xmlProducts )
            {
                var imageurls = new List<string>();
                var postTitle = string.Empty;
                var sku = string.Empty;

                foreach (XmlNode child in xmlProduct.ChildNodes)
                {
                    if (child.Name == "nazwa") postTitle = child.InnerText.Trim();
                    if (child.Name == "kod_katalogowy") sku = child.InnerText.Trim();
                    if (child.Name == "zdjecia") foreach (XmlNode photo in child.ChildNodes) imageurls?.Add(photo.InnerText.Trim());
                }

                parentSkuImageDict.TryAdd(sku, (postTitle, imageurls));
            }

            var parentProductIds = await _dbContext.WpPosts
                .Where(x => x.PostType == "product")
                .Select(x => (long)x.Id)
                .ToListAsync(cancellationToken);

            var productIdSkusDict = await _dbContext.WpWcProductMetaLookups
                .Where(x => parentProductIds.Contains(x.ProductId))?
                .ToDictionaryAsync(x => x.ProductId, x => x.Sku, cancellationToken);

            var imagesWithNames = new List<ImagesWithNamesDto>();

            foreach (var productIdSkuDict in productIdSkusDict)
            {
                parentSkuImageDict.TryGetValue(productIdSkuDict.Value, out var details);
                var productId = productIdSkuDict.Key;

                if (details == (null, null)) continue;

                var imagesWithNameDto = new ImagesWithNamesDto((ulong)productId, details.imageUrls, details.postTitle);
                imagesWithNames.Add(imagesWithNameDto);
            }

            
            var addedImages = await _imageService.AddImagesOnServer(imagesWithNames);

            return Result.Ok();
        }
        catch
        {
            return Result.Fail("Error");
        }
    }

    internal class ProductUpdateData
    {
        public List<ProductImage> Images { get; set; }

        internal class ProductImage
        {
            public string Src { get; set; }
        }
    }
}
