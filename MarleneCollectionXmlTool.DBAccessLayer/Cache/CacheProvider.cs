using MarleneCollectionXmlTool.DBAccessLayer.Cache.Models;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using System.Collections.Immutable;

namespace MarleneCollectionXmlTool.DBAccessLayer.Cache;

public interface ICacheProvider
{
    ImmutableList<WpTerm> GetAllWpTerms();
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

    public ImmutableList<WpTerm> GetAllWpTerms() => GetAnyCacheData<ImmutableList<WpTerm>>("WpTerms");
}
