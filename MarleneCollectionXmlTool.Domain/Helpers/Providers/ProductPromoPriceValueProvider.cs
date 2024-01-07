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

    public ProductPromoPriceValueProvider()
    {
        _priceMarginFactor = 1.0m;
        _priceMarginStatic = 0m;
    }

    public ProductPriceDto GetNewProductPrice(
        decimal catalogPrice, decimal? promoPrice, decimal currentPrice, decimal? currentPromoPrice)
    {
        if (catalogPrice == 0 && promoPrice == 0)
            return new ProductPriceDto(currentPrice, currentPromoPrice);

        if (currentPrice < catalogPrice)
            currentPrice = catalogPrice;

        currentPromoPrice = promoPrice;

        var currentPromoPriceWithMargin = (currentPromoPrice * _priceMarginFactor) + _priceMarginStatic;
        var currentPriceWithMargin = (currentPrice * _priceMarginFactor) + _priceMarginStatic;

        return new ProductPriceDto(currentPriceWithMargin, currentPromoPriceWithMargin);
    }
}
