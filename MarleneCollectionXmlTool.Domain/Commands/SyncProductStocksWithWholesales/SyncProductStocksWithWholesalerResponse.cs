namespace MarleneCollectionXmlTool.Domain.Commands.SyncProductStocksWithWholesales;

public class SyncProductStocksWithWholesalerResponse
{
    public SyncProductStocksWithWholesalerResponse(int updatedProducts, int missingProducts)
    {
        UpdatedProducts = updatedProducts;
        MissingProducts = missingProducts;
    }

    public int UpdatedProducts { get; set; }
    public int MissingProducts { get; set; }
}
