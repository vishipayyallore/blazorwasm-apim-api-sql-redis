using StackExchange.Redis;

namespace BooksStore.CacheDal.Interfaces
{

    public interface IRedisCacheDbContext
    {
        IDatabase RedisDatabase { get; }
    }

}
