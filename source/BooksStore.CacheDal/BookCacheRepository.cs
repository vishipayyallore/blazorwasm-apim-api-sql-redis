using BooksStore.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BooksStore.CacheDal
{

    public class BookCacheRepository : IBookCacheRepository
    {
        private readonly IRedisCacheDbContext _redisCacheDbContext;
        private readonly ILogger<BookCacheRepository> _logger;

        public BookCacheRepository(IRedisCacheDbContext redisCacheDbContext, ILogger<BookCacheRepository> logger)
        {
            _redisCacheDbContext = redisCacheDbContext ?? throw new ArgumentNullException(nameof(redisCacheDbContext));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> RetrieveItemFromCache(string itemKey)
        {
            var itemFromCache = string.Empty;

            try
            {
                _logger.LogInformation("Request Received for RedisCacheDbDal::RetrieveItemFromCache");

                itemFromCache = await _redisCacheDbContext.RedisDatabase.StringGetAsync(itemKey);

                _logger.LogInformation("Returning the results from RedisCacheDbDal::RetrieveItemFromCache");
            }
            catch (Exception error)
            {
                // ToDo: Log into File.
                _logger.LogError($"Error occurred at RedisCacheDbDal::RetrieveItemFromCache(). Message: {error.Message}");
            }

            return itemFromCache;
        }

        public async Task<bool> SaveOrUpdateItemToCache(string itemKey, string itemValue)
        {
            bool itemSavedIntoCache = false;

            try
            {
                _logger.LogInformation("Request Received for RedisCacheDbDal::SaveOrUpdateItemToCache");

                itemSavedIntoCache = await _redisCacheDbContext.RedisDatabase.StringSetAsync(itemKey, itemValue);

                _logger.LogInformation("Returning the results from RedisCacheDbDal::SaveOrUpdateItemToCache");
            }
            catch (Exception error)
            {
                // ToDo: Log into File.
                _logger.LogError($"Error occurred at RedisCacheDbDal::SaveItemToCache(). Message: {error.Message}");
            }

            return itemSavedIntoCache;
        }

        public async Task<bool> DeleteItemFromCache(string itemKey)
        {
            bool itemDeletedFromCache = false;

            try
            {
                _logger.LogInformation("Request Received for RedisCacheDbDal::DeleteItemFromCache");

                itemDeletedFromCache = await _redisCacheDbContext.RedisDatabase.KeyDeleteAsync(itemKey);

                _logger.LogInformation("Returning the results from RedisCacheDbDal::DeleteItemFromCache");
            }
            catch (Exception error)
            {
                // ToDo: Log into File.
                _logger.LogError($"Error occurred at RedisCacheDbDal::DeleteItemFromCache(). Message: {error.Message}");
            }

            return itemDeletedFromCache;
        }

    }

}
