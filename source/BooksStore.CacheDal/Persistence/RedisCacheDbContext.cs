using BooksStore.CacheDal.Interfaces;
using BooksStore.Core.Configuration;
using StackExchange.Redis;

namespace BooksStore.CacheDal.Persistence
{

    public class RedisCacheDbContext : IRedisCacheDbContext
    {

        public RedisCacheDbContext(IDataStoreSettings dataStoreSettings)
        {
            RedisDatabase = ConnectionMultiplexer
                                .Connect(dataStoreSettings.RedisConnectionString)
                                .GetDatabase();
        }

        public IDatabase RedisDatabase { get; }

    }

}
