
//using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _db;
    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }
    public async Task<string?> GetAsync(string key)
    {
        return await _db.StringGetAsync(key);
    }

    public async Task RemoveAsync(string key)
    {
         await _db.KeyDeleteAsync(key);
    }

    public async Task SetAsync(string key, string value, TimeSpan? expiry = null)
    {
        await _db.StringSetAsync(key , value ,expiry );
    }
}