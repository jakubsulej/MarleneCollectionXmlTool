using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Queries.SyncProductStocksWithWholesales.Models;
using System.Collections.Immutable;

namespace MarleneCollectionXmlTool.Domain.Helpers.WpPostHelpers;

public interface IProductAttributeService
{
    string CreateProductAttributesString(WpPostDto parentProductDto, List<WpPostDto> variantProductsDtos);
    Task<ProductAttributesDto> MapParentProductTaxonomyValues(ulong parentProductId, WpPostDto parentProductDto, List<WpPostDto> variableProductDtos, List<WpTerm> terms);
    Task<ProductAttributesDto> MapVariableProductTaxonomyValues(ulong variantProductId, ulong parentProductId, WpPostDto variableProductDto, List<WpTerm> terms);
}

public record ProductAttributesDto(List<WpWcProductAttributesLookup> AttributesLookups, List<WpTermRelationship> Relationships);

public class ProductAttributeService : IProductAttributeService
{
    private readonly WoocommerceDbContext _dbContext;

    public ProductAttributeService(WoocommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string CreateProductAttributesString(WpPostDto parentProductDto, List<WpPostDto> variantProductsDtos)
    {
        var attributes = new Dictionary<string, ProductAttributes.Attribute>();

        AddNewParentProductAttribute(
            attributeName: "pa_rozmiar",
            attributeKey: "pa_rozmiar",
            possition: 0,
            isVisible: true,
            isVariation: true,
            isTaxonomy: true,
            parentAttribute: string.Empty,
            variantProductsDtos: variantProductsDtos,
            attributes: attributes,
            expressionTree: x => x.AttributeKolor);

        AddNewParentProductAttribute(
            attributeName: "pa_kolor",
            attributeKey: "pa_kolor",
            possition: 1,
            isVisible: true,
            isVariation: false,
            isTaxonomy: true,
            parentAttribute: string.Empty,
            variantProductsDtos: variantProductsDtos,
            attributes: attributes,
            expressionTree: x => x.AttributeKolor);

        AddNewParentProductAttribute(
            attributeName: "pa_fason",
            attributeKey: "pa_fason",
            possition: 2,
            isVisible: true,
            isVariation: false,
            isTaxonomy: true,
            parentAttribute: string.Empty,
            variantProductsDtos: variantProductsDtos,
            attributes: attributes,
            expressionTree: x => x.AttributeFason);

        AddNewParentProductAttribute(
            attributeName: "pa_dlugosc",
            attributeKey: "pa_dlugosc",
            possition: 3,
            isVisible: true,
            isVariation: false,
            isTaxonomy: true,
            parentAttribute: string.Empty,
            variantProductsDtos: variantProductsDtos,
            attributes: attributes,
            expressionTree: x => x.AttributeDlugosc);

        AddNewParentProductAttribute(
            attributeName: "pa_wzor",
            attributeKey: "pa_wzor",
            possition: 4,
            isVisible: true,
            isVariation: false,
            isTaxonomy: true,
            parentAttribute: string.Empty,
            variantProductsDtos: variantProductsDtos,
            attributes: attributes,
            expressionTree: x => x.AttributeWzor);

        var productAttributes = new ProductAttributes(attributes);

        var serializedObject = ConvertToPHPArray(productAttributes);
        return serializedObject;
    }

    public static string ConvertToPHPArray(ProductAttributes attributes)
    {
        string serializedString = "a:" + attributes.Attributes.Count + ":{";
        int index = 0;

        foreach (var attribute in attributes.Attributes)
        {
            string name = attribute.Key;
            var value = attribute.Value;

            serializedString += $"s:{name.Length}:\"{name}\";a:6:{{";
            serializedString += $"s:4:\"name\";s:{value.Name.Length}:\"{value.Name}\";";
            serializedString += $"s:5:\"value\";s:{value.Value.Length}:\"{value.Value}\";";
            serializedString += $"s:8:\"position\";i:{value.Position};";
            serializedString += $"s:10:\"is_visible\";i:{value.IsVisible};";
            serializedString += $"s:12:\"is_variation\";i:{value.IsVariation};";
            serializedString += $"s:11:\"is_taxonomy\";i:{value.IsTaxonomy};";
            serializedString += "}";

            index++;
        }

        serializedString += "}";

        return serializedString;
    }

    private static void AddNewParentProductAttribute(string attributeName, string attributeKey, int possition, bool isVisible,
        bool isVariation, bool isTaxonomy, string parentAttribute, List<WpPostDto> variantProductsDtos,
        Dictionary<string, ProductAttributes.Attribute> attributes, Func<WpPostDto, string> expressionTree)
    {
        var wholesalerAttributes = new List<string>();
        wholesalerAttributes.AddRange(variantProductsDtos.Select(expressionTree).Where(x => !string.IsNullOrEmpty(x)));

        if (!string.IsNullOrEmpty(parentAttribute))
            wholesalerAttributes.AddRange(parentAttribute.Split(","));

        MapCustomNamingConvention(attributeKey, wholesalerAttributes);

        var attributeValues = string.Join(" | ", wholesalerAttributes);

        var attribute = new ProductAttributes.Attribute
        {
            Name = attributeName,
            Value = attributeValues,
            Position = possition,
            IsVisible = Convert.ToInt32(isVisible),
            IsVariation = Convert.ToInt32(isVariation),
            IsTaxonomy = Convert.ToInt32(isTaxonomy)
        };

        attributes.Add(attributeKey, attribute);
    }

    private static void MapCustomNamingConvention(string attributeKey, List<string> wholesalerAttributes)
    {
        //disabled until resolved
        //for (int i = 0; i < wholesalerAttributes.Count; i++)
        //{
        //    wholesalerAttributes[i] = _valueMappingService.GetCustomAttributeMappingValue(attributeKey, wholesalerAttributes[i]);
        //}
        
        for (int i = 0; i < wholesalerAttributes.Count; i++)
        {
            wholesalerAttributes[i] = wholesalerAttributes[i].FirstLetterToUpper();
        }
    }

    public async Task<ProductAttributesDto> MapParentProductTaxonomyValues(
        ulong parentProductId, WpPostDto parentProductDto, List<WpPostDto> variableProductDtos, List<WpTerm> terms)
    {
        var rozmiarAttributeValues = variableProductDtos.SelectMany(x => x.AttributeRozmiar?.Split(","))?.ToList();
        var fasonAttributeValues = parentProductDto.AttributeFason?.Split(",")?.ToList();
        var kolorAttributeValues = parentProductDto.AttributeKolor?.Split(",")?.ToList();
        var wzorAttributeValues = parentProductDto.AttributeWzor?.Split(",")?.ToList();
        var dlugoscAttributeValues = parentProductDto.AttributeDlugosc?.Split(",")?.ToList();

        var rozmiarTermIds = await MapTaxonomyRelationship("pa_rozmiar", rozmiarAttributeValues, terms);
        var fasonTermIds = await MapTaxonomyRelationship("pa_fason", fasonAttributeValues, terms);
        var kolorTermIds = await MapTaxonomyRelationship("pa_kolor", kolorAttributeValues, terms);
        var wzorTermIds = await MapTaxonomyRelationship("pa_wzor", wzorAttributeValues, terms);
        var dlugoscTermIds = await MapTaxonomyRelationship("pa_dlugosc", dlugoscAttributeValues, terms);

        var productAttributesLookup = new List<WpWcProductAttributesLookup>();
        var termRelationships = new List<WpTermRelationship>();

        var rozmiarLookups = MapAttributesLookupAndRelations(parentProductId, parentProductId, rozmiarTermIds, "pa_rozmiar");
        var fasonLookups = MapAttributesLookupAndRelations(parentProductId, parentProductId, fasonTermIds, "pa_fason");
        var kolorLookups = MapAttributesLookupAndRelations(parentProductId, parentProductId, kolorTermIds, "pa_kolor");
        var wzorLookups = MapAttributesLookupAndRelations(parentProductId, parentProductId, wzorTermIds, "pa_wzor");
        var dlugoscLookups = MapAttributesLookupAndRelations(parentProductId, parentProductId, dlugoscTermIds, "pa_dlugosc");

        termRelationships.Add(new WpTermRelationship(parentProductId, 4)); //variable

        productAttributesLookup.AddRange(rozmiarLookups.attributesLookups);
        termRelationships.AddRange(rozmiarLookups.wpTermRelationships);

        productAttributesLookup.AddRange(fasonLookups.attributesLookups);
        termRelationships.AddRange(fasonLookups.wpTermRelationships);

        productAttributesLookup.AddRange(kolorLookups.attributesLookups);
        termRelationships.AddRange(kolorLookups.wpTermRelationships);

        productAttributesLookup.AddRange(wzorLookups.attributesLookups);
        termRelationships.AddRange(wzorLookups.wpTermRelationships);

        productAttributesLookup.AddRange(dlugoscLookups.attributesLookups);
        termRelationships.AddRange(dlugoscLookups.wpTermRelationships);

        if (parentProductDto.StockStatus == "outofstock") 
            termRelationships.Add(new WpTermRelationship(parentProductId, 9));

        var distinctRelations = new List<WpTermRelationship>();

        foreach (var relation in termRelationships)
        {
            if (distinctRelations.Any(x => x.ObjectId == relation.ObjectId && x.TermTaxonomyId == relation.TermTaxonomyId))
                continue;

            distinctRelations.Add(relation);
        }

        return new ProductAttributesDto(productAttributesLookup, distinctRelations);
    }

    public async Task<ProductAttributesDto> MapVariableProductTaxonomyValues(
        ulong variantProductId, ulong parentProductId, WpPostDto variableProductDto, List<WpTerm> terms)
    {
        var rozmiarAttributeValues = variableProductDto.AttributeRozmiar?.Split(",").ToList();
        var fasonAttributeValues = variableProductDto.AttributeFason?.Split(",")?.ToList();
        var kolorAttributeValues = variableProductDto.AttributeKolor?.Split(",")?.ToList();
        var wzorAttributeValues = variableProductDto.AttributeWzor?.Split(",")?.ToList();
        var dlugoscAttributeValues = variableProductDto.AttributeDlugosc?.Split(",")?.ToList();

        var rozmiarTermIds = await MapTaxonomyRelationship("pa_rozmiar", rozmiarAttributeValues, terms);
        var fasonTermIds = await MapTaxonomyRelationship("pa_fason", fasonAttributeValues, terms);
        var kolorTermIds = await MapTaxonomyRelationship("pa_kolor", kolorAttributeValues, terms);
        var wzorTermIds = await MapTaxonomyRelationship("pa_wzor", wzorAttributeValues, terms);
        var dlugoscTermIds = await MapTaxonomyRelationship("pa_dlugosc", dlugoscAttributeValues, terms);

        var productAttributesLookup = new List<WpWcProductAttributesLookup>();
        var termRelationships = new List<WpTermRelationship>();

        var rozmiarLookups = MapAttributesLookupAndRelations(variantProductId, parentProductId, rozmiarTermIds, "pa_rozmiar");
        var fasonLookups = MapAttributesLookupAndRelations(variantProductId, parentProductId, fasonTermIds, "pa_fason");
        var kolorLookups = MapAttributesLookupAndRelations(variantProductId, parentProductId, kolorTermIds, "pa_kolor");
        var wzorLookups = MapAttributesLookupAndRelations(variantProductId, parentProductId, wzorTermIds, "pa_wzor");
        var dlugoscLookups = MapAttributesLookupAndRelations(variantProductId, parentProductId, dlugoscTermIds, "pa_dlugosc");

        productAttributesLookup.AddRange(rozmiarLookups.attributesLookups);
        termRelationships.AddRange(rozmiarLookups.wpTermRelationships);

        productAttributesLookup.AddRange(fasonLookups.attributesLookups);
        termRelationships.AddRange(fasonLookups.wpTermRelationships);

        productAttributesLookup.AddRange(kolorLookups.attributesLookups);
        termRelationships.AddRange(kolorLookups.wpTermRelationships);

        productAttributesLookup.AddRange(wzorLookups.attributesLookups);
        termRelationships.AddRange(wzorLookups.wpTermRelationships);

        productAttributesLookup.AddRange(dlugoscLookups.attributesLookups);
        termRelationships.AddRange(dlugoscLookups.wpTermRelationships);

        if (variableProductDto.StockStatus == "outofstock")
            termRelationships.Add(new WpTermRelationship(variantProductId, 9));

        return new ProductAttributesDto(productAttributesLookup, termRelationships);
    }

    private async Task<List<ulong>> MapTaxonomyRelationship(string taxonomyName, List<string> attributeValues, List<WpTerm> terms)
    {
        try
        {
            var termTaxonomyIds = new List<ulong>();

            if (attributeValues == null)
                return termTaxonomyIds;

            foreach (var attribute in attributeValues)
            {
                if (string.IsNullOrEmpty(attribute)) continue;

                var attributeName = attribute.ToUpper();
                if (attribute.ToUpper() == "GREEN") attributeName = "ZIELONY";
                if (attribute.ToUpper() == "CREAM") attributeName = "ECRU";
                if (attribute.ToUpper() == "CZARNA") attributeName = "CZARNY";
                if (attribute.ToUpper() == "W PASKI") attributeName = "PASKI";
                if (attribute.ToUpper() == "ŚMIETANKOWY") attributeName = "ECRU";
                if (attribute.ToUpper() == "DŁUGI RĘKAW") attributeName = "DŁUGIE";
                if (attribute.ToUpper() == "RĘKAW ¾") attributeName = "RĘKAW 3/4";
                if (attribute.ToUpper() == "CZARNO-BIAŁY") attributeName = "BIAŁO-CZARNY";
                if (attribute.ToUpper() == "W PEPITKĘ") attributeName = "PEPITKA";

                var taxonomyId = terms?
                    .Where(x => x.Name.ToUpper() == attributeName)
                    .FirstOrDefault()?
                    .TermId;

                if (taxonomyId.HasValue == false)
                {
                    var wpTerm = await AddMissingWpTermAsync(attributeName, taxonomyName);
                    terms.Add(wpTerm);
                    taxonomyId = wpTerm.TermId;
                }

                termTaxonomyIds.Add(taxonomyId.Value);
            }

            return termTaxonomyIds;
        }
        catch (Exception ex) 
        { 
            return new List<ulong>();
        }
    }

    private async Task<WpTerm> AddMissingWpTermAsync(string termValue, string taxonomyName)
    {
        try
        {
            var wpTerm = new WpTerm
            {
                Name = termValue.FirstLetterToUpper(),
                Slug = termValue.GenerateSlug(),
                TermGroup = 0,
            };

            await _dbContext.AddAsync(wpTerm);
            await _dbContext.SaveChangesAsync();

            var wpTermTaxonomy = new WpTermTaxonomy
            {
                TermId = wpTerm.TermId,
                Taxonomy = taxonomyName,
                Description = string.Empty,
                Parent = 0,
                Count = 0,
            };

            await _dbContext.AddAsync(wpTermTaxonomy);
            await _dbContext.SaveChangesAsync();

            return wpTerm;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private static (List<WpWcProductAttributesLookup> attributesLookups, List<WpTermRelationship> wpTermRelationships) MapAttributesLookupAndRelations(
        ulong variantProductId, ulong parentProductId, List<ulong> attributeTermIds, string taxonomyName)
    {
        var productAttributesLookup = new List<WpWcProductAttributesLookup>();
        var termRelationships = new List<WpTermRelationship>();

        foreach (var attributeTermId in attributeTermIds)
        {
            termRelationships.Add(new WpTermRelationship
            {
                ObjectId = variantProductId,
                TermTaxonomyId = attributeTermId,
            });

            productAttributesLookup.Add(new WpWcProductAttributesLookup
            {
                ProductId = (long)variantProductId,
                ProductOrParentId = (long)parentProductId,
                Taxonomy = taxonomyName,
                TermId = (long)attributeTermId,
                IsVariationAttribute = variantProductId != parentProductId,
                InStock = true
            });
        }

        return (productAttributesLookup, termRelationships);
    }

    public class ProductAttributes
    {
        public ProductAttributes(Dictionary<string, Attribute> attributes)
        {
            Attributes = attributes;
        }

        public Dictionary<string, Attribute> Attributes { get; }

        public class Attribute
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public int Position { get; set; }
            public int IsVisible { get; set; }
            public int IsVariation { get; set; }
            public int IsTaxonomy { get; set; }
        }
    }
}
