using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Utils;

namespace MarleneCollectionXmlTool.Domain.Tests.Utils;

internal static class MockDataHelper
{
    public record WpPostWithMeta(WpPost WpPost, List<WpPostmetum> WpPostmetum);

    internal static List<WpPostWithMeta> GetFakeProductWithVariations()
    {
        var sku = "D20-ZIELON";
        var variantTree = new Dictionary<string, List<(string sku, string attributeRozmiar, string stock, string price, string stockStatus)>>
        {
            {
                sku,
                new ()
                {
                    { ("5908214227099", "xs-s", "3", "123", "instock") },
                    { ("5908214227082", "m-l", "5", "123", "instock") },
                    { ("5908214231799", "xl-xxl", "11", "123", "instock") },
                    { ("590821423180", "3xl-4xl", "0", "123", "outofstock") },
                    { ("5908214231812", "5xl-6xl", "0", "123", "outofstock") },
                }
            },
        };

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
                    PostStatus = "publish",
                    PostName = "dummy post name",
                    PostParent = 0,
                    Guid = string.Empty,
                    PostType = "product",
                },
                new List<WpPostmetum>
                {
                    new WpPostmetum(1, MetaKeyConstrains.Sku, sku),
                    new WpPostmetum(1, MetaKeyConstrains.ManageStock, "no"),
                    new WpPostmetum(1, MetaKeyConstrains.Stock, null),
                    new WpPostmetum(1, MetaKeyConstrains.StockStatus, "instock"),
                    new WpPostmetum(1, MetaKeyConstrains.ProductAttributes, "\"a:5:{s:7:\\\"rozmiar\\\";a:6:{s:4:\\\"name\\\";s:7:\\\"Rozmiar\\\";s:5:\\\"value\\\";s:39:\\\"XS/S | M/L | XL/XXL | 3XL/4XL | 5XL/6XL\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:1;s:11:\\\"is_taxonomy\\\";i:0;}s:5:\\\"kolor\\\";a:6:{s:4:\\\"name\\\";s:5:\\\"Kolor\\\";s:5:\\\"value\\\";s:7:\\\"Bordowy\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:0;s:11:\\\"is_taxonomy\\\";i:0;}s:5:\\\"fason\\\";a:6:{s:4:\\\"name\\\";s:5:\\\"Fason\\\";s:5:\\\"value\\\";s:26:\\\"Z kieszeniami | Z kapturem\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:0;s:11:\\\"is_taxonomy\\\";i:0;}s:7:\\\"dlugosc\\\";a:6:{s:4:\\\"name\\\";s:7:\\\"Długość\\\";s:5:\\\"value\\\";s:6:\\\"Długie\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:0;s:11:\\\"is_taxonomy\\\";i:0;}s:4:\\\"wzor\\\";a:6:{s:4:\\\"name\\\";s:4:\\\"Wzór\\\";s:5:\\\"value\\\";s:13:\\\"Jednokolorowe\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:0;s:11:\\\"is_taxonomy\\\";i:0;}}\", \r\n"),
                    new WpPostmetum(1, MetaKeyConstrains.HasParent, "no"),
                    new WpPostmetum(1, MetaKeyConstrains.Price, "123"),
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
                    PostStatus = "publish",
                    PostName = "dummy post name",
                    PostParent = 1,
                    Guid = string.Empty,
                    PostType = "product_variation",
                },
                new List<WpPostmetum>
                {
                    new WpPostmetum(2, MetaKeyConstrains.VariantDescription, string.Empty),
                    new WpPostmetum(2, MetaKeyConstrains.ManageStock, "yes"),
                    new WpPostmetum(2, MetaKeyConstrains.Stock, variantTree[sku][0].stock),
                    new WpPostmetum(2, MetaKeyConstrains.StockStatus, variantTree[sku][1].stockStatus),
                    new WpPostmetum(2, MetaKeyConstrains.AttributePaRozmiar, variantTree[sku][0].attributeRozmiar),
                    new WpPostmetum(2, MetaKeyConstrains.Sku, variantTree[sku][0].sku),
                    new WpPostmetum(2, MetaKeyConstrains.RegularPrice, variantTree[sku][0].price),
                    new WpPostmetum(2, MetaKeyConstrains.Price, variantTree[sku][0].price),
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
                    PostStatus = "publish",
                    PostName = "dummy post name",
                    PostParent = 1,
                    Guid = string.Empty,
                    PostType = "product_variation",
                },
                new List<WpPostmetum>
                {
                    new WpPostmetum(3, MetaKeyConstrains.VariantDescription, string.Empty),
                    new WpPostmetum(3, MetaKeyConstrains.ManageStock, "yes"),
                    new WpPostmetum(3, MetaKeyConstrains.Stock, variantTree[sku][1].stock),
                    new WpPostmetum(3, MetaKeyConstrains.StockStatus, variantTree[sku][1].stockStatus),
                    new WpPostmetum(3, MetaKeyConstrains.AttributePaRozmiar, variantTree[sku][1].attributeRozmiar),
                    new WpPostmetum(3, MetaKeyConstrains.Sku, variantTree[sku][1].sku),
                    new WpPostmetum(3, MetaKeyConstrains.RegularPrice, variantTree[sku][1].price),
                    new WpPostmetum(3, MetaKeyConstrains.Price, variantTree[sku][1].price),
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
                    PostStatus = "publish",
                    PostName = "dummy post name",
                    PostParent = 1,
                    Guid = string.Empty,
                    PostType = "product_variation",

                },
                new List<WpPostmetum>
                {
                    new WpPostmetum(4, MetaKeyConstrains.VariantDescription, string.Empty),
                    new WpPostmetum(4, MetaKeyConstrains.ManageStock, "yes"),
                    new WpPostmetum(4, MetaKeyConstrains.Stock, variantTree[sku][2].stock),
                    new WpPostmetum(4, MetaKeyConstrains.StockStatus, variantTree[sku][2].stockStatus),
                    new WpPostmetum(4, MetaKeyConstrains.AttributePaRozmiar, variantTree[sku][2].attributeRozmiar),
                    new WpPostmetum(4, MetaKeyConstrains.Sku, variantTree[sku][2].sku),
                    new WpPostmetum(4, MetaKeyConstrains.RegularPrice, variantTree[sku][2].price),
                    new WpPostmetum(4, MetaKeyConstrains.Price, variantTree[sku][2].price),
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
                    PostStatus = "publish",
                    PostName = "dummy post name",
                    PostParent = 1,
                    Guid = string.Empty,
                    PostType = "product_variation",
                },
                new List<WpPostmetum>
                {
                    new WpPostmetum(5, MetaKeyConstrains.VariantDescription, string.Empty),
                    new WpPostmetum(5, MetaKeyConstrains.ManageStock, "yes"),
                    new WpPostmetum(5, MetaKeyConstrains.Stock, variantTree[sku][3].stock),
                    new WpPostmetum(5, MetaKeyConstrains.StockStatus, variantTree[sku][3].stockStatus),
                    new WpPostmetum(5, MetaKeyConstrains.AttributePaRozmiar, variantTree[sku][3].attributeRozmiar),
                    new WpPostmetum(5, MetaKeyConstrains.Sku, variantTree[sku][3].sku),
                    new WpPostmetum(5, MetaKeyConstrains.RegularPrice, variantTree[sku][3].price),
                    new WpPostmetum(5, MetaKeyConstrains.Price, variantTree[sku][3].price),
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
                    PostStatus = "publish",
                    PostName = "dummy post name",
                    PostParent = 1,
                    Guid = string.Empty,
                    PostType = "product_variation",
                },
                new List<WpPostmetum>
                {
                    new WpPostmetum(6, MetaKeyConstrains.VariantDescription, string.Empty),
                    new WpPostmetum(6, MetaKeyConstrains.ManageStock, "yes"),
                    new WpPostmetum(6, MetaKeyConstrains.Stock, variantTree[sku][4].stock),
                    new WpPostmetum(6, MetaKeyConstrains.StockStatus, variantTree[sku][4].stockStatus),
                    new WpPostmetum(6, MetaKeyConstrains.AttributePaRozmiar, variantTree[sku][4].attributeRozmiar),
                    new WpPostmetum(6, MetaKeyConstrains.Sku, variantTree[sku][4].sku),
                    new WpPostmetum(6, MetaKeyConstrains.RegularPrice, variantTree[sku][4].price),
                    new WpPostmetum(6, MetaKeyConstrains.Price, variantTree[sku][4].price),
                }
            )
        };
    }
}
