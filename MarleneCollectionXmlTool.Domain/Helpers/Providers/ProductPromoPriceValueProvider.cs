namespace MarleneCollectionXmlTool.Domain.Helpers.Providers;

public interface IProductPromoPriceValueProvider
{
    (decimal newRegularPrice, decimal? newPromoPrice) GetNewProductPrice(decimal catalogPrice, decimal? promoPrice, decimal currentPrice, decimal? currentPromoPrice);
}

public class ProductPromoPriceValueProvider : IProductPromoPriceValueProvider
{
    private readonly decimal _priceMarginFactor;

    public ProductPromoPriceValueProvider()
    {
        _priceMarginFactor = 1.0m;
    }

    public (decimal newRegularPrice, decimal? newPromoPrice) GetNewProductPrice(
        decimal catalogPrice, decimal? promoPrice, decimal currentPrice, decimal? currentPromoPrice)
    {
        if (catalogPrice == 0 && promoPrice == 0)
            return (currentPrice, currentPromoPrice);

        if (currentPrice < catalogPrice)
            currentPrice = catalogPrice;

        currentPromoPrice = promoPrice;

        var currentPromoPriceWithMargin = currentPromoPrice * _priceMarginFactor;
        var currentPriceWithMargin = currentPrice * _priceMarginFactor;

        return (currentPriceWithMargin, currentPromoPriceWithMargin);
    }
}
