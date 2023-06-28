namespace MarleneCollectionXmlTool.DBAccessLayer.Cache.Models;

internal static class CacheSemaphores
{
    internal static readonly SemaphoreSlim GetWpTerms = new(1, 1);
}
