using FluentResults;
using MarleneCollectionXmlTool.Domain.Services.ClientSevices;
using MarleneCollectionXmlTool.Domain.Utils;
using MediatR;
using System.Xml;

namespace MarleneCollectionXmlTool.Domain.Queries.CountTotalNumberOfUniqueItems;

public class CountTotalNumberOfUniqueItemsRequestHandler : IRequestHandler<CountTotalNumberOfUniqueItemsRequest, Result<CountTotalNumberOfUniqueItemsResponse>>
{
    private readonly IGetXmlDocumentFromWholesalerService _wholesalerService;

    public CountTotalNumberOfUniqueItemsRequestHandler(IGetXmlDocumentFromWholesalerService wholesalerService)
    {
        _wholesalerService = wholesalerService;
    }

    public async Task<Result<CountTotalNumberOfUniqueItemsResponse>> Handle(CountTotalNumberOfUniqueItemsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var xmlDoc = await _wholesalerService.GetXmlDocumentNestedVariantsXmlUrl(cancellationToken);
            var xmlProducts = xmlDoc.GetElementsByTagName(HurtIvonXmlConstrains.Produkt);

            var uniqueSkus = new List<string>();

            foreach (XmlNode xmlProduct in xmlProducts)
            {
                foreach (XmlNode child in xmlProduct.ChildNodes)
                {
                    if (child.Name == HurtIvonXmlConstrains.KodKatalogowy)
                    {
                        uniqueSkus.Add(child.Value.Trim());
                        break;
                    }
                }
            }

            var result = new CountTotalNumberOfUniqueItemsResponse(uniqueSkus.Count);
            return Result.Ok(result);
        }
        catch
        {
            return Result.Fail("Error");
        }
    }
}
