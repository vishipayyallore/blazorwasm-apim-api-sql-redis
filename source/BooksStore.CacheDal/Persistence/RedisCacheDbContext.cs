using BooksStore.CacheDal.Interfaces;
using StackExchange.Redis;

namespace BooksStore.CacheDal.Persistence
{

    public class RedisCacheDbContext : IRedisCacheDbContext
    {

        public RedisCacheDbContext(ConnectionMultiplexer connectionMultiplexer)
        {
            RedisDatabase = connectionMultiplexer.GetDatabase();
        }

        public IDatabase RedisDatabase { get; }

    }

}
