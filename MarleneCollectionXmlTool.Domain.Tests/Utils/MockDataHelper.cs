using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Utils;

namespace MarleneCollectionXmlTool.Domain.Tests.Utils;

internal static class MockDataHelper
{
    internal record WpPostWithMeta(WpPost WpPost, List<WpPostmetum> WpPostmetum, WpWcProductMetaLookup ProductMetaLookup);
    internal record FakeProductVariableValues(string Sku, string AttributeRozmiar, string Stock, string Price, string RegularPrice, string? SalesPrice, string StockStatus);

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
        var variantTree = customVariantTree ?? _defaultVariantTree;

        var results = new List<WpPostWithMeta>();

        var lastPostId = 1u;
        foreach (var product in variantTree)
        {
            var parentId = lastPostId;

            var postsWithMeta = new List<WpPostWithMeta>
            {
                new WpPostWithMeta
                (
                    new WpPost
                    {
                        Id = parentId,
                        PostContent = "dummy content",
                        PostTitle = "dummy post title",
                        PostExcerpt = "dummy post exceprt",
                        PostStatus = WpPostConstans.Publish,
                        PostName = "dummy post name",
                        PostParent = 0,
                        Guid = string.Empty,
                        PostType = "product",
                    },
                    new List<WpPostmetum>
                    {
                        new WpPostmetum(parentId, MetaKeyConstans.Sku, product.Key),
                        new WpPostmetum(parentId, MetaKeyConstans.ManageStock, MetaValueConstans.No),
                        new WpPostmetum(parentId, MetaKeyConstans.Stock, null),
                        new WpPostmetum(parentId, MetaKeyConstans.StockStatus, MetaValueConstans.InStock),
                        new WpPostmetum(parentId, MetaKeyConstans.ProductAttributes, "\"a:5:{s:7:\\\"rozmiar\\\";a:6:{s:4:\\\"name\\\";s:7:\\\"Rozmiar\\\";s:5:\\\"value\\\";s:39:\\\"XS/S | M/L | XL/XXL | 3XL/4XL | 5XL/6XL\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:1;s:11:\\\"is_taxonomy\\\";i:0;}s:5:\\\"kolor\\\";a:6:{s:4:\\\"name\\\";s:5:\\\"Kolor\\\";s:5:\\\"value\\\";s:7:\\\"Bordowy\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:0;s:11:\\\"is_taxonomy\\\";i:0;}s:5:\\\"fason\\\";a:6:{s:4:\\\"name\\\";s:5:\\\"Fason\\\";s:5:\\\"value\\\";s:26:\\\"Z kieszeniami | Z kapturem\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:0;s:11:\\\"is_taxonomy\\\";i:0;}s:7:\\\"dlugosc\\\";a:6:{s:4:\\\"name\\\";s:7:\\\"Długość\\\";s:5:\\\"value\\\";s:6:\\\"Długie\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:0;s:11:\\\"is_taxonomy\\\";i:0;}s:4:\\\"wzor\\\";a:6:{s:4:\\\"name\\\";s:4:\\\"Wzór\\\";s:5:\\\"value\\\";s:13:\\\"Jednokolorowe\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:0;s:11:\\\"is_taxonomy\\\";i:0;}}\", \r\n"),
                        new WpPostmetum(parentId, MetaKeyConstans.HasParent, MetaValueConstans.No),
                        new WpPostmetum(parentId, MetaKeyConstans.Price, "123"),
                    },
                    new WpWcProductMetaLookup
                    {
                        ProductId = parentId,
                        Sku = product.Key,
                        Virtual = false,
                        Downloadable = false,
                        MinPrice = 123,
                        MaxPrice = 123,
                        Onsale = false,
                        StockQuantity = 0,
                        StockStatus = MetaValueConstans.InStock,
                        RatingCount = null,
                        AverageRating = null,
                        TotalSales = 0,
                        TaxStatus = MetaValueConstans.Taxable
                    }
                )
            };
            lastPostId++;

            var variantIndex = 0;
            foreach (var variation in product.Value)
            {
                var variantId = lastPostId;

                var variantPostsWithMeta = new WpPostWithMeta
                (
                    new WpPost
                    {
                        Id = variantId,
                        PostContent = "dummy content",
                        PostTitle = "dummy post title",
                        PostExcerpt = "dummy post exceprt",
                        PostStatus = WpPostConstans.Publish,
                        PostName = "dummy post name",
                        PostParent = parentId,
                        Guid = string.Empty,
                        PostType = "product_variation",
                    },
                    new List<WpPostmetum>
                    {
                        new WpPostmetum(variantId, MetaKeyConstans.VariantDescription, string.Empty),
                        new WpPostmetum(variantId, MetaKeyConstans.ManageStock, MetaValueConstans.Yes),
                        new WpPostmetum(variantId, MetaKeyConstans.Stock, variantTree[product.Key][variantIndex].Stock),
                        new WpPostmetum(variantId, MetaKeyConstans.StockStatus, variantTree[product.Key][variantIndex].StockStatus),
                        new WpPostmetum(variantId, MetaKeyConstans.AttributePaRozmiar, variantTree[product.Key][variantIndex].AttributeRozmiar),
                        new WpPostmetum(variantId, MetaKeyConstans.Sku, variantTree[product.Key][variantIndex].Sku),
                        new WpPostmetum(variantId, MetaKeyConstans.RegularPrice, variantTree[product.Key][variantIndex].Price),
                        new WpPostmetum(variantId, MetaKeyConstans.Price, variantTree[product.Key][variantIndex].Price)
                    },
                    new WpWcProductMetaLookup
                    {
                        ProductId = variantId,
                        Sku = variantTree[product.Key][variantIndex].Sku,
                        Virtual = false,
                        Downloadable = false,
                        MinPrice = decimal.Parse(variantTree[product.Key][variantIndex].Price),
                        MaxPrice = decimal.Parse(variantTree[product.Key][variantIndex].Price),
                        Onsale = false,
                        StockQuantity = double.Parse(variantTree[product.Key][variantIndex].Stock),
                        StockStatus = variantTree[product.Key][variantIndex].StockStatus,
                        RatingCount = null,
                        AverageRating = null,
                        TotalSales = 0,
                        TaxStatus = MetaValueConstans.Taxable
                    }
                );

                if (variantTree[product.Key][variantIndex].SalesPrice != null)
                {
                    variantPostsWithMeta.WpPostmetum
                        .Add(new WpPostmetum(variantId, MetaKeyConstans.SalePrice, variantTree[product.Key][variantIndex].SalesPrice));
                }

                postsWithMeta.Add(variantPostsWithMeta);

                variantIndex++;
                lastPostId++;
            }

            parentId++;

            results.AddRange(postsWithMeta);
        }

        return results;
    }
}
