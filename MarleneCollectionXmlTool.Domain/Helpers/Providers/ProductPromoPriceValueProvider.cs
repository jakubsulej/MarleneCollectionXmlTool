namespace MarleneCollectionXmlTool.Domain.Helpers.Providers;

public interface IProductPromoPriceValueProvider
{
    ProductPriceDto GetNewProductPrice(decimal catalogPrice, decimal? promoPrice, decimal currentPrice, decimal? currentPromoPrice);
}

public record ProductPriceDto(decimal RegularPrice, decimal? PromoPrice);

public class ProductPromoPriceValueProvider : IProductPromoPriceValueProvider
{
    private readonly decimal _priceMarginFactor;
    private readonly decimal _priceMarginStatic;

    public ProductPromoPriceValueProvider(decimal priceMarginFactor, decimal priceMarginStatic)
    {
        _priceMarginFactor = priceMarginFactor;
        _priceMarginStatic = priceMarginStatic;
    }

    public ProductPriceDto GetNewProductPrice(
        decimal catalogRegularPrice, decimal? catalogPromoPrice, decimal currentRegularPrice, decimal? currentPromoPrice)
    {
        if (catalogRegularPrice == 0 && catalogPromoPrice == 0)
            return new ProductPriceDto(currentRegularPrice, currentPromoPrice);

        currentRegularPrice = catalogRegularPrice;
        currentPromoPrice = catalogPromoPrice;
        var currentPromoPriceWithMargin = (currentPromoPrice * _priceMarginFactor) + _priceMarginStatic;
        var catalogPriceWithMargin = (currentRegularPrice * _priceMarginFactor) + _priceMarginStatic;

        return new ProductPriceDto(catalogPriceWithMargin, currentPromoPriceWithMargin);
    }
}
