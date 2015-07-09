namespace Nz.Co.Dunkley.Cornerman.Api.Models
{
    using System;
    using System.Collections.Generic;
    using MongoDB.Bson;
    using MongoRepository;

    public class Ride : Entity
    {
        public Ride()
        {
            Wingmen = new List<Rider>();
            Riders = new List<Rider>();
        }

        public Guid RideId { get; set; }
        public BsonDateTime StartDateTime { get; set; }
        public BsonDateTime CompletedDateTime { get; set; }
        public Rider Lead { get; set; }
        public Rider Tailgunner { get; set; }
        public List<Rider> Wingmen { get; set; }
        public List<Rider> Riders { get; set; }
    }
}