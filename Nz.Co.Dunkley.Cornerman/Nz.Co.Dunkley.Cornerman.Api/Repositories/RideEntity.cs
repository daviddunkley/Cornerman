namespace Nz.Co.Dunkley.Cornerman.Api.Repositories
{
    using System.Collections.Generic;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class RideEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id {get;set;}
        public string Title { get; set; }
        public BsonDateTime StartDateTime { get; set; }
        public BsonDateTime CompletedDateTime { get; set; }
        public RiderEntity Lead { get; set; }
        public RiderEntity Tailgunner { get; set; }
        public List<RiderEntity> Wingmen { get; set; }
        public List<RiderEntity> Riders { get; set; }
    }
}