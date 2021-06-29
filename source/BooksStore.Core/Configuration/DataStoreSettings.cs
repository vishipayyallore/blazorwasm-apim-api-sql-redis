namespace BooksStore.Core.Configuration
{

    public class DataStoreSettings : IDataStoreSettings
    {
        public string SqlServerConnectionString { get; set; }

        public string RedisConnectionString { get; set; }
    }

}
