using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Utils;

namespace MarleneCollectionXmlTool.Domain.Tests.Utils;

internal static class MockDataHelper
{
    internal record WpPostWithMeta(WpPost WpPost, List<WpPostmetum> WpPostmetum, WpWcProductMetaLookup ProductMetaLookup);
    internal record FakeProductVariableValues(string Sku, string AttributeRozmiar, string Stock, string Price, string RegularPrice, string SalesPrice, string StockStatus);

    private static readonly Dictionary<string, List<FakeProductVariableValues>> _defaultVariantTree = new()
    {
        {
            "D20-ZIELON",
            new List<FakeProductVariableValues>
            {
                new FakeProductVariableValues("5908214227099", "xs-s", "3", "123", "123", "123", "instock"),
                new FakeProductVariableValues("5908214227082", "m-l", "5", "123", "123", "123", "instock"),
                new FakeProductVariableValues("5908214231799", "xl-xxl", "11", "123", "123", "123", "instock"),
                new FakeProductVariableValues("590821423180", "3xl-4xl", "0", "123", "123", "123", "outofstock"),
                new FakeProductVariableValues("5908214231812", "5xl-6xl", "0", "123", "123", "123", "outofstock"),
            }
        },
    };

    internal static List<WpPostWithMeta> GetFakeProductWithVariations(Dictionary<string, List<FakeProductVariableValues>>? customVariantTree = null)
    {
        var sku = "D20-ZIELON";
        var variantTree = customVariantTree ?? _defaultVariantTree;

        return new List<WpPostWithMeta>
        {
            new WpPostWithMeta
            (
                new WpPost
                {
                    Id = 1,
                    PostContent = "dummy content",
                    PostTitle = "dummy post title",
                    PostExcerpt = "dummy post exceprt",
                    PostStatus = WpPostConstrains.Publish,
                    PostName = "dummy post name",
                    PostParent = 0,
                    Guid = string.Empty,
                    PostType = "product",
                },
                new List<WpPostmetum>
                {
                    new WpPostmetum(1, MetaKeyConstrains.Sku, sku),
                    new WpPostmetum(1, MetaKeyConstrains.ManageStock, MetaValueConstrains.No),
                    new WpPostmetum(1, MetaKeyConstrains.Stock, null),
                    new WpPostmetum(1, MetaKeyConstrains.StockStatus, MetaValueConstrains.InStock),
                    new WpPostmetum(1, MetaKeyConstrains.ProductAttributes, "\"a:5:{s:7:\\\"rozmiar\\\";a:6:{s:4:\\\"name\\\";s:7:\\\"Rozmiar\\\";s:5:\\\"value\\\";s:39:\\\"XS/S | M/L | XL/XXL | 3XL/4XL | 5XL/6XL\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:1;s:11:\\\"is_taxonomy\\\";i:0;}s:5:\\\"kolor\\\";a:6:{s:4:\\\"name\\\";s:5:\\\"Kolor\\\";s:5:\\\"value\\\";s:7:\\\"Bordowy\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:0;s:11:\\\"is_taxonomy\\\";i:0;}s:5:\\\"fason\\\";a:6:{s:4:\\\"name\\\";s:5:\\\"Fason\\\";s:5:\\\"value\\\";s:26:\\\"Z kieszeniami | Z kapturem\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:0;s:11:\\\"is_taxonomy\\\";i:0;}s:7:\\\"dlugosc\\\";a:6:{s:4:\\\"name\\\";s:7:\\\"Długość\\\";s:5:\\\"value\\\";s:6:\\\"Długie\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:0;s:11:\\\"is_taxonomy\\\";i:0;}s:4:\\\"wzor\\\";a:6:{s:4:\\\"name\\\";s:4:\\\"Wzór\\\";s:5:\\\"value\\\";s:13:\\\"Jednokolorowe\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:0;s:11:\\\"is_taxonomy\\\";i:0;}}\", \r\n"),
                    new WpPostmetum(1, MetaKeyConstrains.HasParent, MetaValueConstrains.No),
                    new WpPostmetum(1, MetaKeyConstrains.Price, "123"),
                },
                new WpWcProductMetaLookup
                {
                    ProductId = 1,
                    Sku = sku,
                    Virtual = false,
                    Downloadable = false,
                    MinPrice = 123,
                    MaxPrice = 123,
                    Onsale = false,
                    StockQuantity = 0,
                    StockStatus = MetaValueConstrains.InStock,
                    RatingCount = null,
                    AverageRating = null,
                    TotalSales = 0,
                    TaxStatus = MetaValueConstrains.Taxable
                }
            ),
            new WpPostWithMeta
            (
                new WpPost
                {
                    Id = 2,
                    PostContent = "dummy content",
                    PostTitle = "dummy post title",
                    PostExcerpt = "dummy post exceprt",
                    PostStatus = WpPostConstrains.Publish,
                    PostName = "dummy post name",
                    PostParent = 1,
                    Guid = string.Empty,
                    PostType = "product_variation",
                },
                new List<WpPostmetum>
                {
                    new WpPostmetum(2, MetaKeyConstrains.VariantDescription, string.Empty),
                    new WpPostmetum(2, MetaKeyConstrains.ManageStock, MetaValueConstrains.Yes),
                    new WpPostmetum(2, MetaKeyConstrains.Stock, variantTree[sku][0].Stock),
                    new WpPostmetum(2, MetaKeyConstrains.StockStatus, variantTree[sku][1].StockStatus),
                    new WpPostmetum(2, MetaKeyConstrains.AttributePaRozmiar, variantTree[sku][0].AttributeRozmiar),
                    new WpPostmetum(2, MetaKeyConstrains.Sku, variantTree[sku][0].Sku),
                    new WpPostmetum(2, MetaKeyConstrains.RegularPrice, variantTree[sku][0].Price),
                    new WpPostmetum(2, MetaKeyConstrains.Price, variantTree[sku][0].Price),
                    new WpPostmetum(2, MetaKeyConstrains.SalePrice, variantTree[sku][0].SalesPrice),
                },
                new WpWcProductMetaLookup
                {
                    ProductId = 2,
                    Sku = variantTree[sku][0].Sku,
                    Virtual = false,
                    Downloadable = false,
                    MinPrice = decimal.Parse(variantTree[sku][0].SalesPrice),
                    MaxPrice = decimal.Parse(variantTree[sku][0].Price),
                    Onsale = false,
                    StockQuantity = double.Parse(variantTree[sku][0].Stock),
                    StockStatus = variantTree[sku][0].StockStatus,
                    RatingCount = null,
                    AverageRating = null,
                    TotalSales = 0,
                    TaxStatus = MetaValueConstrains.Taxable
                }
            ),
            new WpPostWithMeta
            (
                new WpPost
                {
                    Id = 3,
                    PostContent = "dummy content",
                    PostTitle = "dummy post title",
                    PostExcerpt = "dummy post exceprt",
                    PostStatus = WpPostConstrains.Publish,
                    PostName = "dummy post name",
                    PostParent = 1,
                    Guid = string.Empty,
                    PostType = "product_variation",
                },
                new List<WpPostmetum>
                {
                    new WpPostmetum(3, MetaKeyConstrains.VariantDescription, string.Empty),
                    new WpPostmetum(3, MetaKeyConstrains.ManageStock, MetaValueConstrains.Yes),
                    new WpPostmetum(3, MetaKeyConstrains.Stock, variantTree[sku][1].Stock),
                    new WpPostmetum(3, MetaKeyConstrains.StockStatus, variantTree[sku][1].StockStatus),
                    new WpPostmetum(3, MetaKeyConstrains.AttributePaRozmiar, variantTree[sku][1].AttributeRozmiar),
                    new WpPostmetum(3, MetaKeyConstrains.Sku, variantTree[sku][1].Sku),
                    new WpPostmetum(3, MetaKeyConstrains.RegularPrice, variantTree[sku][1].Price),
                    new WpPostmetum(3, MetaKeyConstrains.Price, variantTree[sku][1].Price),
                    new WpPostmetum(3, MetaKeyConstrains.SalePrice, variantTree[sku][1].SalesPrice),
                },
                new WpWcProductMetaLookup
                {
                    ProductId = 3,
                    Sku = variantTree[sku][1].Sku,
                    Virtual = false,
                    Downloadable = false,
                    MinPrice = decimal.Parse(variantTree[sku][1].SalesPrice),
                    MaxPrice = decimal.Parse(variantTree[sku][1].Price),
                    Onsale = false,
                    StockQuantity = double.Parse(variantTree[sku][1].Stock),
                    StockStatus = variantTree[sku][1].StockStatus,
                    RatingCount = null,
                    AverageRating = null,
                    TotalSales = 0,
                    TaxStatus = MetaValueConstrains.Taxable
                }
            ),
            new WpPostWithMeta
            (
                new WpPost
                {
                    Id = 4,
                    PostContent = "dummy content",
                    PostTitle = "dummy post title",
                    PostExcerpt = "dummy post exceprt",
                    PostStatus = WpPostConstrains.Publish,
                    PostName = "dummy post name",
                    PostParent = 1,
                    Guid = string.Empty,
                    PostType = "product_variation",

                },
                new List<WpPostmetum>
                {
                    new WpPostmetum(4, MetaKeyConstrains.VariantDescription, string.Empty),
                    new WpPostmetum(4, MetaKeyConstrains.ManageStock, MetaValueConstrains.Yes),
                    new WpPostmetum(4, MetaKeyConstrains.Stock, variantTree[sku][2].Stock),
                    new WpPostmetum(4, MetaKeyConstrains.StockStatus, variantTree[sku][2].StockStatus),
                    new WpPostmetum(4, MetaKeyConstrains.AttributePaRozmiar, variantTree[sku][2].AttributeRozmiar),
                    new WpPostmetum(4, MetaKeyConstrains.Sku, variantTree[sku][2].Sku),
                    new WpPostmetum(4, MetaKeyConstrains.RegularPrice, variantTree[sku][2].Price),
                    new WpPostmetum(4, MetaKeyConstrains.Price, variantTree[sku][2].Price),
                    new WpPostmetum(4, MetaKeyConstrains.SalePrice, variantTree[sku][2].SalesPrice),
                },
                new WpWcProductMetaLookup
                {
                    ProductId = 4,
                    Sku = variantTree[sku][2].Sku,
                    Virtual = false,
                    Downloadable = false,
                    MinPrice = decimal.Parse(variantTree[sku][2].SalesPrice),
                    MaxPrice = decimal.Parse(variantTree[sku][2].Price),
                    Onsale = false,
                    StockQuantity = double.Parse(variantTree[sku][2].Stock),
                    StockStatus = variantTree[sku][2].StockStatus,
                    RatingCount = null,
                    AverageRating = null,
                    TotalSales = 0,
                    TaxStatus = MetaValueConstrains.Taxable
                }
            ),
            new WpPostWithMeta
            (
                new WpPost
                {
                    Id = 5,
                    PostContent = "dummy content",
                    PostTitle = "dummy post title",
                    PostExcerpt = "dummy post exceprt",
                    PostStatus = WpPostConstrains.Publish,
                    PostName = "dummy post name",
                    PostParent = 1,
                    Guid = string.Empty,
                    PostType = "product_variation",
                },
                new List<WpPostmetum>
                {
                    new WpPostmetum(5, MetaKeyConstrains.VariantDescription, string.Empty),
                    new WpPostmetum(5, MetaKeyConstrains.ManageStock, MetaValueConstrains.Yes),
                    new WpPostmetum(5, MetaKeyConstrains.Stock, variantTree[sku][3].Stock),
                    new WpPostmetum(5, MetaKeyConstrains.StockStatus, variantTree[sku][3].StockStatus),
                    new WpPostmetum(5, MetaKeyConstrains.AttributePaRozmiar, variantTree[sku][3].AttributeRozmiar),
                    new WpPostmetum(5, MetaKeyConstrains.Sku, variantTree[sku][3].Sku),
                    new WpPostmetum(5, MetaKeyConstrains.RegularPrice, variantTree[sku][3].Price),
                    new WpPostmetum(5, MetaKeyConstrains.Price, variantTree[sku][3].Price),
                    new WpPostmetum(5, MetaKeyConstrains.SalePrice, variantTree[sku][3].SalesPrice),
                },  
                new WpWcProductMetaLookup
                {
                    ProductId = 5,
                    Sku = variantTree[sku][3].Sku,
                    Virtual = false,
                    Downloadable = false,
                    MinPrice = decimal.Parse(variantTree[sku][3].SalesPrice),
                    MaxPrice = decimal.Parse(variantTree[sku][3].Price),
                    Onsale = false,
                    StockQuantity = double.Parse(variantTree[sku][3].Stock),
                    StockStatus = variantTree[sku][3].StockStatus,
                    RatingCount = null,
                    AverageRating = null,
                    TotalSales = 0,
                    TaxStatus = MetaValueConstrains.Taxable
                }
            ),
            new WpPostWithMeta
            (
                new WpPost
                {
                    Id = 6,
                    PostContent = "dummy content",
                    PostTitle = "dummy post title",
                    PostExcerpt = "dummy post exceprt",
                    PostStatus = WpPostConstrains.Publish,
                    PostName = "dummy post name",
                    PostParent = 1,
                    Guid = string.Empty,
                    PostType = "product_variation",
                },
                new List<WpPostmetum>
                {
                    new WpPostmetum(6, MetaKeyConstrains.VariantDescription, string.Empty),
                    new WpPostmetum(6, MetaKeyConstrains.ManageStock, MetaValueConstrains.Yes),
                    new WpPostmetum(6, MetaKeyConstrains.Stock, variantTree[sku][4].Stock),
                    new WpPostmetum(6, MetaKeyConstrains.StockStatus, variantTree[sku][4].StockStatus),
                    new WpPostmetum(6, MetaKeyConstrains.AttributePaRozmiar, variantTree[sku][4].AttributeRozmiar),
                    new WpPostmetum(6, MetaKeyConstrains.Sku, variantTree[sku][4].Sku),
                    new WpPostmetum(6, MetaKeyConstrains.RegularPrice, variantTree[sku][4].Price),
                    new WpPostmetum(6, MetaKeyConstrains.Price, variantTree[sku][4].Price),
                    new WpPostmetum(6, MetaKeyConstrains.SalePrice, variantTree[sku][4].SalesPrice),
                },
                new WpWcProductMetaLookup
                {
                    ProductId = 6,
                    Sku = variantTree[sku][4].Sku,
                    Virtual = false,
                    Downloadable = false,
                    MinPrice = decimal.Parse(variantTree[sku][4].SalesPrice),
                    MaxPrice = decimal.Parse(variantTree[sku][4].Price),
                    Onsale = false,
                    StockQuantity = double.Parse(variantTree[sku][4].Stock),
                    StockStatus = variantTree[sku][4].StockStatus,
                    RatingCount = null,
                    AverageRating = null,
                    TotalSales = 0,
                    TaxStatus = MetaValueConstrains.Taxable
                }
            ),
        };
    }
}
