using BooksStore.Core.Configuration;
using BooksStore.Core.Interfaces;
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
