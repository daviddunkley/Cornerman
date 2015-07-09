namespace Nz.Co.Dunkley.Cornerman.Api.Repositories
{
    using System;
    using Microsoft.WindowsAzure.Storage.Table;

    public class RidePointEntity : TableEntity
    {
        public DateTime PointDateTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
    }
}