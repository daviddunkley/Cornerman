namespace Nz.Co.Dunkley.Cornerman.Api.Repositories
{
    using Microsoft.WindowsAzure.Storage.Table;

    public class RideEntity : TableEntity
    {
        public string JsonData { get; set; }
    }
}