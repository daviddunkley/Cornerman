namespace Nz.Co.Dunkley.Cornerman.Api.Repositories
{
    using Microsoft.Azure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    public class AzureStorageContext
    {
        private readonly CloudTable _ridesTable; 
        private readonly CloudTable _riderPointsTable;

        public AzureStorageContext()
        {
            var connectionString =
                CloudConfigurationManager.GetSetting("AzureStorage.ConnectionString");

            var storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudTableClient client = storageAccount.CreateCloudTableClient();

            _ridesTable = client.GetTableReference("Rides");
            _ridesTable.CreateIfNotExists();
            
            _riderPointsTable = client.GetTableReference("RiderPoints");
            _riderPointsTable.CreateIfNotExists();            
        }

        public CloudTable RidesTable
        {
            get { return _ridesTable; }
        }

        public CloudTable RiderPointsTable
        {
            get { return _riderPointsTable; }
        }
    }
}