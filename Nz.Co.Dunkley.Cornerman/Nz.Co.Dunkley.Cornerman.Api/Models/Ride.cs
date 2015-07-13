namespace Nz.Co.Dunkley.Cornerman.Api.Models
{
    using System;
    using System.Collections.Generic;
    using MongoDB.Bson;

    [Serializable]
    public class Ride
    {
        public Ride()
        {
            _id = ObjectId.GenerateNewId().ToString();
            Wingmen = new List<Rider>();
            Riders = new List<Rider>();
        }

        public string _id { get; set; }
        public string RideId { get; set; }
        public string Title { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime CompletedDateTime { get; set; }
        public Rider Lead { get; set; }
        public Rider Tailgunner { get; set; }
        public List<Rider> Wingmen { get; set; }
        public List<Rider> Riders { get; set; }
    }
}