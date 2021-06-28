using StackExchange.Redis;

namespace Books.API.CacheDal.Persistence
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
