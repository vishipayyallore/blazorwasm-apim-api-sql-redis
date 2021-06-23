using Books.APIV1.Interfaces;

namespace Books.APIV1.Configuration
{

    public class DataStoreSettings : IDataStoreSettings
    {
        public DataStoreSettings(string sqlServerConnection) => SqlServerConnectionString = sqlServerConnection;

        public string SqlServerConnectionString { get; }
    }

}
