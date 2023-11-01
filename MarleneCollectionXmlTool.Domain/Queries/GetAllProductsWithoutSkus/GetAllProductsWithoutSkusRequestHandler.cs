using FluentResults;
using MarleneCollectionXmlTool.Domain.Services.ClientSevices;
using MarleneCollectionXmlTool.Domain.Utils;
using MediatR;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text;
using System.Xml;

namespace MarleneCollectionXmlTool.Domain.Queries.GetAllProductsWithoutSkus;

public class GetAllProductsWithoutSkusRequestHandler : IRequestHandler<GetAllProductsWithoutSkusRequest, Result<GetAllProductsWithoutSkusResponse>>
{
    private readonly IGetXmlDocumentFromWholesalerService _wholesalerService;

    public GetAllProductsWithoutSkusRequestHandler(IGetXmlDocumentFromWholesalerService wholesalerService)
    {
        _wholesalerService = wholesalerService;
    }

    public async Task<Result<GetAllProductsWithoutSkusResponse>> Handle(GetAllProductsWithoutSkusRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var xmlDoc = await _wholesalerService.GetXmlDocumentNestedVariantsXmlUrl(cancellationToken);
            var xmlProducts = xmlDoc.GetElementsByTagName(HurtIvonXmlConstans.Produkt);

            var productsWithMissingSkus = GetAllProductsWithMissingSkus(xmlProducts);

            var output = new StringBuilder();

            foreach (var product in productsWithMissingSkus)
            {
                var parentSb = new StringBuilder()
                    .Append(product.Id)
                    .Append(";")
                    .Append(product.ProductName)
                    .Append(";")
                    .Append(product.CatalogeCode)
                    .Append(";");

                foreach (var variant in product.VariantProducts)
                {
                    var variantSb = new StringBuilder()
                        .Append(variant.Id)
                        .Append(";")
                        .Append(variant.Size);

                    var row = new StringBuilder()
                        .Append(parentSb)
                        .Append(variantSb)
                        .ToString();

                    output.AppendLine(row);
                }
            }

            var response = new GetAllProductsWithoutSkusResponse
            {
                //ProductWithoutSkuRow = output.ToString(),
                ProductsWithoutSkus = productsWithMissingSkus
            };

            return Result.Ok(response);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    private static List<ParentProductDto> GetAllProductsWithMissingSkus(XmlNodeList xmlProducts)
    {
        var productsWithMissingSkus = new List<ParentProductDto>();

        foreach (XmlNode xmlProduct in xmlProducts)
        {
            var parentProductDto = new ParentProductDto();
            var variants = null as XmlNodeList;

            foreach (XmlNode parent in xmlProduct.ChildNodes)
            {
                if (parent.Name == HurtIvonXmlConstans.Id) parentProductDto.Id = parent.InnerText.Trim();
                if (parent.Name == HurtIvonXmlConstans.Nazwa) parentProductDto.ProductName = parent.InnerText.Trim();
                if (parent.Name == HurtIvonXmlConstans.KodKatalogowy) parentProductDto.CatalogeCode = parent.InnerText.Trim();
                if (parent.Name == HurtIvonXmlConstans.Warianty) variants = parent.ChildNodes;
            }

            foreach (XmlNode variant in variants)
            {
                var variantProductDto = new ParentProductDto.VariantProductDto();

                foreach (XmlNode child in variant.ChildNodes)
                {
                    if (child.Name == HurtIvonXmlConstans.Id) variantProductDto.Id = child.InnerText.Trim();
                    if (child.Name == HurtIvonXmlConstans.Ean) variantProductDto.Sku = child.InnerText.Trim();
                    if (child.Name == HurtIvonXmlConstans.Rozmiar) variantProductDto.Size = child.InnerText.Trim();
                }

                if (string.IsNullOrWhiteSpace(variantProductDto.Sku))
                    parentProductDto.VariantProducts.Add(variantProductDto);
            }

            if (parentProductDto.VariantProducts.Any())
                productsWithMissingSkus.Add(parentProductDto);
        }

        return productsWithMissingSkus;
    }
}
