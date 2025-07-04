﻿using MarleneCollectionXmlTool.Domain.Utils;
using System.Xml;

namespace MarleneCollectionXmlTool.Domain.Helpers.Providers;

public static class ProductSkuProvider
{
    public static string GetVariantProductSku(XmlNodeList variantChildNodes, string variantProductEan)
    {
        if (!string.IsNullOrWhiteSpace(variantProductEan))
            return variantProductEan.Trim();

        foreach (XmlNode child in variantChildNodes)
        {
            if (child.Name == HurtIvonXmlConstans.Id)
                return child.InnerText.Trim();
        }

        return string.Empty;
    }
}
