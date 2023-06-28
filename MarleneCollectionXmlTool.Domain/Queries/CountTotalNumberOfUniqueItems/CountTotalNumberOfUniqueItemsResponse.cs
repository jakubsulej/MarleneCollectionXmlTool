namespace MarleneCollectionXmlTool.Domain.Queries.CountTotalNumberOfUniqueItems;

public class CountTotalNumberOfUniqueItemsResponse
{
    public CountTotalNumberOfUniqueItemsResponse(int numberOfItems)
    {
        TotalNumberOfItems = numberOfItems;
    }
    public int TotalNumberOfItems { get; set; }
}
