using MarleneCollectionXmlTool.DBAccessLayer.Models;

namespace MarleneCollectionXmlTool.Domain.Tests.Utils;

internal static class MockDataHelper
{
    internal static List<WpPost> GetFakeProductWithVariations()
    {
        var variantTree = new Dictionary<string, List<(string sku, string attributeRozmiar, string stock, string price, string stockStatus)>>
        {
            {
                "D20-ZIELON",
                new ()
                {
                    { ("5908214227099", "XS/S", "3", "123", "instock") },
                    { ("5908214227082", "M/L", "5", "123", "instock") },
                    { ("5908214231799", "XL/XXL", "11", "123", "instock") },
                    { ("590821423180", "3XL/4XL", "0", "123", "outofstock") },
                    { ("5908214231812", "5XL/6XL", "0", "123", "outofstock") },
                }
            },
        };

        return new List<WpPost>
        {
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
                //WpPostMetas = new List<WpPostmetum>
                //{
                //    new WpPostmetum(1, "_sku", "D20-ZIELON"),
                //    new WpPostmetum(1, "_manage_stock", "no"),
                //    new WpPostmetum(1, "_stock", null),
                //    new WpPostmetum(1, "_stock_status", "instock"),
                //    new WpPostmetum(1, "_product_attributes", "\"a:5:{s:7:\\\"rozmiar\\\";a:6:{s:4:\\\"name\\\";s:7:\\\"Rozmiar\\\";s:5:\\\"value\\\";s:39:\\\"XS/S | M/L | XL/XXL | 3XL/4XL | 5XL/6XL\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:1;s:11:\\\"is_taxonomy\\\";i:0;}s:5:\\\"kolor\\\";a:6:{s:4:\\\"name\\\";s:5:\\\"Kolor\\\";s:5:\\\"value\\\";s:7:\\\"Bordowy\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:0;s:11:\\\"is_taxonomy\\\";i:0;}s:5:\\\"fason\\\";a:6:{s:4:\\\"name\\\";s:5:\\\"Fason\\\";s:5:\\\"value\\\";s:26:\\\"Z kieszeniami | Z kapturem\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:0;s:11:\\\"is_taxonomy\\\";i:0;}s:7:\\\"dlugosc\\\";a:6:{s:4:\\\"name\\\";s:7:\\\"Długość\\\";s:5:\\\"value\\\";s:6:\\\"Długie\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:0;s:11:\\\"is_taxonomy\\\";i:0;}s:4:\\\"wzor\\\";a:6:{s:4:\\\"name\\\";s:4:\\\"Wzór\\\";s:5:\\\"value\\\";s:13:\\\"Jednokolorowe\\\";s:8:\\\"position\\\";i:1;s:10:\\\"is_visible\\\";i:1;s:12:\\\"is_variation\\\";i:0;s:11:\\\"is_taxonomy\\\";i:0;}}\", \r\n"),
                //    new WpPostmetum(1, "has_parent", "no"),
                //    new WpPostmetum(1, "_price", "123"),
                //}
            },
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
                //WpPostMetas = new List<WpPostmetum>
                //{
                //    new WpPostmetum(2, "_variation_description", string.Empty),
                //    new WpPostmetum(2, "_manage_stock", "yes"),
                //    new WpPostmetum(2, "_stock", variantTree["D20-ZIELON"][0].stock),
                //    new WpPostmetum(2, "_stock_status", variantTree["D20-ZIELON"][1].stockStatus),
                //    new WpPostmetum(2, "attribute_rozmiar", variantTree["D20-ZIELON"][0].attributeRozmiar),
                //    new WpPostmetum(2, "_sku", variantTree["D20-ZIELON"][0].sku),
                //    new WpPostmetum(2, "_regular_price", variantTree["D20-ZIELON"][0].price),
                //    new WpPostmetum(2, "_price", variantTree["D20-ZIELON"][0].price),
                //}
            },
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
                //WpPostMetas = new List<WpPostmetum>
                //{
                //    new WpPostmetum(3, "_variation_description", string.Empty),
                //    new WpPostmetum(3, "_manage_stock", "yes"),
                //    new WpPostmetum(3, "_stock", variantTree["D20-ZIELON"][1].stock),
                //    new WpPostmetum(3, "_stock_status", variantTree["D20-ZIELON"][1].stockStatus),
                //    new WpPostmetum(3, "attribute_rozmiar", variantTree["D20-ZIELON"][1].attributeRozmiar),
                //    new WpPostmetum(3, "_sku", variantTree["D20-ZIELON"][1].sku),
                //    new WpPostmetum(3, "_regular_price", variantTree["D20-ZIELON"][1].price),
                //    new WpPostmetum(3, "_price", variantTree["D20-ZIELON"][1].price),
                //}
            },
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
                //WpPostMetas = new List<WpPostmetum>
                //{
                //    new WpPostmetum(4, "_variation_description", string.Empty),
                //    new WpPostmetum(4, "_manage_stock", "yes"),
                //    new WpPostmetum(4, "_stock", variantTree["D20-ZIELON"][2].stock),
                //    new WpPostmetum(4, "_stock_status", variantTree["D20-ZIELON"][2].stockStatus),
                //    new WpPostmetum(4, "attribute_rozmiar", variantTree["D20-ZIELON"][2].attributeRozmiar),
                //    new WpPostmetum(4, "_sku", variantTree["D20-ZIELON"][2].sku),
                //    new WpPostmetum(4, "_regular_price", variantTree["D20-ZIELON"][2].price),
                //    new WpPostmetum(4, "_price", variantTree["D20-ZIELON"][2].price),
                //}
            },
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
                //WpPostMetas = new List<WpPostmetum>
                //{
                //    new WpPostmetum(5, "_variation_description", string.Empty),
                //    new WpPostmetum(5, "_manage_stock", "yes"),
                //    new WpPostmetum(5, "_stock", variantTree["D20-ZIELON"][3].stock),
                //    new WpPostmetum(5, "_stock_status", variantTree["D20-ZIELON"][3].stockStatus),
                //    new WpPostmetum(5, "attribute_rozmiar", variantTree["D20-ZIELON"][3].attributeRozmiar),
                //    new WpPostmetum(5, "_sku", variantTree["D20-ZIELON"][3].sku),
                //    new WpPostmetum(5, "_regular_price", variantTree["D20-ZIELON"][3].price),
                //    new WpPostmetum(5, "_price", variantTree["D20-ZIELON"][3].price),
                //}
            },
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
                //WpPostMetas = new List<WpPostmetum>
                //{
                //    new WpPostmetum(6, "_variation_description", string.Empty),
                //    new WpPostmetum(6, "_manage_stock", "yes"),
                //    new WpPostmetum(6, "_stock", variantTree["D20-ZIELON"][4].stock),
                //    new WpPostmetum(6, "_stock_status", variantTree["D20-ZIELON"][4].stockStatus),
                //    new WpPostmetum(6, "attribute_rozmiar", variantTree["D20-ZIELON"][4].attributeRozmiar),
                //    new WpPostmetum(6, "_sku", variantTree["D20-ZIELON"][4].sku),
                //    new WpPostmetum(6, "_regular_price", variantTree["D20-ZIELON"][4].price),
                //    new WpPostmetum(6, "_price", variantTree["D20-ZIELON"][4].price),
                //}
            },
        };
    }
}
