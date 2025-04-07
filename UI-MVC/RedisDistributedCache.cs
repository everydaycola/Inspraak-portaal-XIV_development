using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

public class RedisDistributedCache : IDistributedCache
{
    private readonly IDistributedCache _cache;

    public RedisDistributedCache(IDistributedCache cache)
    {
        _cache = cache;
    }

    public byte[] Get(string key)
    {
        return _cache.Get(key);
    }

    public async Task<byte[]> GetAsync(string key, System.Threading.CancellationToken token = default)
    {
        return await _cache.GetAsync(key, token);
    }

    public void Refresh(string key)
    {
        _cache.Refresh(key);
    }

    public async Task RefreshAsync(string key, System.Threading.CancellationToken token = default)
    {
        await _cache.RefreshAsync(key, token);
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    public async Task RemoveAsync(string key, System.Threading.CancellationToken token = default)
    {
        await _cache.RemoveAsync(key, token);
    }

    public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        _cache.Set(key, value, options);
    }

    public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options,
        System.Threading.CancellationToken token = default)
    {
        await _cache.SetAsync(key, value, options, token);
    }
}

public static class RedisIdentityServiceCollectionExtensions
{
    public static IServiceCollection AddRedisIdentity(this IServiceCollection services)
    {
        services.AddScoped<IDistributedCache, RedisDistributedCache>();
        return services;
    }
}