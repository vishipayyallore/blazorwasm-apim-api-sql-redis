using System.Threading.Tasks;

namespace Books.API.CacheDal
{

    public interface IBookCacheRepository
    {
        Task<string> RetrieveItemFromCache(string itemKey);

        Task<bool> SaveOrUpdateItemToCache(string itemKey, string itemValue);

        Task<bool> DeleteItemFromCache(string itemKey);
    }

}
