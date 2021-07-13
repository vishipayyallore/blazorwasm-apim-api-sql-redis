using System.Threading.Tasks;

namespace BooksStore.Core.Interfaces
{

    public interface IBookCacheRepository
    {
        Task<string> RetrieveItemFromCache(string itemKey);

        Task<bool> SaveOrUpdateItemToCache(string itemKey, string itemValue);

        Task<bool> DeleteItemFromCache(string itemKey);
    }

}
