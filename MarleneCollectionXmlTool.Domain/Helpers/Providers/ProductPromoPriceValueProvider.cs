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
        decimal catalogPrice, decimal? promoPrice, decimal currentPrice, decimal? currentPromoPrice)
    {
        if (catalogPrice == 0 && promoPrice == 0)
            return new ProductPriceDto(currentPrice, currentPromoPrice);

        currentPrice = catalogPrice;
        currentPromoPrice = promoPrice;
        var currentPromoPriceWithMargin = (currentPromoPrice * _priceMarginFactor) + _priceMarginStatic;
        var catalogPriceWithMargin = (currentPrice * _priceMarginFactor) + _priceMarginStatic;

        return new ProductPriceDto(catalogPriceWithMargin, currentPromoPriceWithMargin);
    }
}
