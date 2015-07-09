namespace Nz.Co.Dunkley.Cornerman.Api.Repositories
{
    using Microsoft.Azure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    public class AzureStorageHelper
    {
        public static CloudTable CreateTable(string tableName)
        {
            var connectionString =
                CloudConfigurationManager.GetSetting("AzureStorage.ConnectionString");

            var storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudTableClient client = storageAccount.CreateCloudTableClient();

            CloudTable table = client.GetTableReference(tableName);

            table.CreateIfNotExists();

            return table;
        }
    }
}