namespace BooksStore.Core.Configuration
{

    public class SettingsData
    {
        public SettingsData(string sqlServerConnection) => SqlServerConnectionString = sqlServerConnection;

        public string SqlServerConnectionString { get; }
    }

}
