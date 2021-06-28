using StackExchange.Redis;

namespace Books.API.CacheDal.Persistence
{

    public interface IRedisCacheDbContext
    {
        IDatabase RedisDatabase { get; }
    }

}
