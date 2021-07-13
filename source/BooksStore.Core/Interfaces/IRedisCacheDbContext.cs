using StackExchange.Redis;

namespace BooksStore.Core.Interfaces
{

    public interface IRedisCacheDbContext
    {
        IDatabase RedisDatabase { get; }
    }

}
