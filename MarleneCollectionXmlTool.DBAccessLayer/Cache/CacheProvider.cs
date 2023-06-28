using MarleneCollectionXmlTool.DBAccessLayer.Cache.Models;
using MarleneCollectionXmlTool.DBAccessLayer.Models;

namespace MarleneCollectionXmlTool.DBAccessLayer.Cache;

public interface ICacheProvider
{
    List<WpTerm> GetAllWpTerms();
}

public class CacheProvider : ICacheProvider
{
    private readonly ICacheDataRepository _dataRepository;
    private readonly CacheData _cacheData;

    public CacheProvider(ICacheDataRepository dataRepository)
    {
        _dataRepository = dataRepository;
        _cacheData = _dataRepository.GetCacheDataAsync().Result;
    }

    private T GetAnyCacheData<T>(string cacheKey)
    {
        try
        {
            return (T)_cacheData.GetType().GetProperty(cacheKey).GetValue(_cacheData, null);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public List<WpTerm> GetAllWpTerms() => GetAnyCacheData<List<WpTerm>>("WpTerms");
}
