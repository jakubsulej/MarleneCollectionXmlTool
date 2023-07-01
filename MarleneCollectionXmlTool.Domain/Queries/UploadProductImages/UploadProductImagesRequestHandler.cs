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
    private readonly WoocommerceDbContext _dbContext;
    private readonly IImageService _imageService;

    public UploadProductImagesRequestHandler(
        IGetXmlDocumentFromWholesalerService wholesalerService,
        WoocommerceDbContext dbContext, 
        IImageService imageService)
    {
        _wholesalerService = wholesalerService;
        _dbContext = dbContext;
        _imageService = imageService;
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

            var skus = await _dbContext.WpWcProductMetaLookups
                .Where(x => parentProductIds.Contains(x.ProductId))?
                .Select(x => x.Sku)?
                .ToListAsync(cancellationToken);

            foreach (var sku in skus)
            {
                parentSkuImageDict.TryGetValue(sku, out var details);

                if (details == (null, null)) continue;

                var postDto = new WpPostDto
                {
                    PostTitle = details.postTitle,
                    Sku = sku,
                    ImageUrls = details.imageUrls
                };

                var postWithMetadata = await _imageService.AddImagesForProduct(postDto);
            }

            return Result.Ok();
        }
        catch
        {
            return Result.Fail("Error");
        }
    }
}
