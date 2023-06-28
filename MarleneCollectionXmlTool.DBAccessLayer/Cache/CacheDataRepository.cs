using MarleneCollectionXmlTool.DBAccessLayer.Cache.Models;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Immutable;

namespace MarleneCollectionXmlTool.DBAccessLayer.Cache;

public interface ICacheDataRepository
{
    Task<CacheData> GetCacheDataAsync();
}

public class CacheDataRepository : ICacheDataRepository
{
    private readonly WoocommerceDbContext _dbContext;
    private readonly IMemoryCache _cache;

    public CacheDataRepository(WoocommerceDbContext dbContext, IMemoryCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<CacheData> GetCacheDataAsync() => new CacheData()
    {
        WpTerms = await GetDataCacheResponse(CacheKeys.WpTerms, CacheSemaphores.GetWpTerms, 5, 2) as ImmutableList<WpTerm>,
    };

    private async Task<object> GetDataCacheResponse(string cacheKey, SemaphoreSlim semaphore, int absoluteExpiration = 20, int slidingExpiration = 10)
    {
        bool isAvailable = _cache.TryGetValue(cacheKey, out object result);

        if (isAvailable) return result;

        try
        {
            await semaphore.WaitAsync();

            result = await GetCacheDataFronDb(cacheKey);
            var cacheEntryOptions = GetMemoryCacheEntryOptions(absoluteExpiration, slidingExpiration);

            _cache.Set(cacheKey, result, cacheEntryOptions);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            semaphore.Release();
        }

        return result;
    }

    private async Task<object> GetCacheDataFronDb(string cacheKey) => cacheKey switch
    {
        CacheKeys.WpTerms => await GetAllWpTermsAsync(),
        _ => throw new NotImplementedException(),
    };

    private async Task<ImmutableList<WpTerm>> GetAllWpTermsAsync()
    {
        var wpTerms = await _dbContext
            .WpTerms
            .ToListAsync();

        return wpTerms.ToImmutableList();
    }

    private static MemoryCacheEntryOptions GetMemoryCacheEntryOptions(int absoluteExpiration, int slidingExpiration, int size = 30000000) => new()
    {
        AbsoluteExpiration = DateTime.Now.AddMinutes(absoluteExpiration),
        SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration),
        Size = size,
    };
}
