namespace Nz.Co.Dunkley.Cornerman.Api.Models
{
    using System;
    using System.Collections.Generic;
    using Microsoft.WindowsAzure.Storage.Blob.Protocol;

    public class Rider
    {
        public Guid RiderId { get; set; }
        public Guid MembershipId { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public List<RidePoint> RidePoints { get; set; }
    }
}