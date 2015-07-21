namespace Nz.Co.Dunkley.Cornerman.Api.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Models;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;

    public class RideRepositoryMongoDb : IRideRepository
    {
        private readonly MongoDbContext _context;

        public RideRepositoryMongoDb(MongoDbContext context = null)
        {
            _context = context ?? new MongoDbContext();
        }

        public string Upsert(Ride ride)
        {
            ride.Created = ride.Created ?? DateTime.UtcNow;
            ride.LastModified = ride.LastModified ?? DateTime.UtcNow;

            var rideDocument = ride.ToBsonDocument();

            _context.Rides.InsertOneAsync(rideDocument).GetAwaiter().GetResult();

            return ride.RideId;
        }

        public string UpsertLead(string rideId, Rider lead)
        {
            var leadDocument = lead.ToBsonDocument();

            var filter = Builders<BsonDocument>.Filter.Eq("RideId", rideId);
            var update = Builders<BsonDocument>.Update
                .Set("Lead", leadDocument)
                .CurrentDate("LastModified");
            var result = _context.Rides.UpdateOneAsync(filter, update, new UpdateOptions() { IsUpsert = false }).Result;
            return lead.RiderId;
        }

        public string UpsertTailgunner(string rideId, Rider tailgunner)
        {
            var tailgunnerDocument = tailgunner.ToBsonDocument();

            var filter = Builders<BsonDocument>.Filter.Eq("RideId", rideId);
            var update = Builders<BsonDocument>.Update
                .Set("Tailgunner", tailgunnerDocument)
                .CurrentDate("LastModified");
            var result = _context.Rides.UpdateOneAsync(filter, update, new UpdateOptions() { IsUpsert = true }).Result;
            return tailgunner.RiderId;
        }

        public string UpsertWingman(string rideId, Rider wingman)
        {
            var wingmanDocument = wingman.ToBsonDocument();

            var filter = Builders<BsonDocument>.Filter.Eq("RideId", rideId);
            var update = Builders<BsonDocument>.Update
                .Push("Wingmen", wingmanDocument)
                .CurrentDate("LastModified");
            var result = _context.Rides.UpdateOneAsync(filter, update, new UpdateOptions() { IsUpsert = true }).Result;
            return wingman.RiderId;
        }

        public string UpsertRider(string rideId, Rider rider)
        {
            var riderDocument = rider.ToBsonDocument();

            var filter = Builders<BsonDocument>.Filter.Eq("RideId", rideId);
            var update = Builders<BsonDocument>.Update
                .Push("Riders", riderDocument)
                .CurrentDate("LastModified");
            var result = _context.Rides.UpdateOneAsync(filter, update, new UpdateOptions() { IsUpsert = true }).Result;
            return rider.RiderId;
        }

        public Ride Retrieve(string rideId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("RideId", rideId);

            Ride ride = null;

            var rows = _context.Rides.FindAsync(filter).Result;

            using (var cursor = rows)
            {
                while (cursor.MoveNextAsync().Result)
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        ride = BsonSerializer.Deserialize<Ride>(document);
                    }
                }
            }

            return ride;
        }

        public List<Ride> RetrieveForMembershipId(string membershipId)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("Tailergunner.MembershipId", membershipId) | 
                builder.Eq("Lead.MembershipId", membershipId);

            List<Ride> rides = new List<Ride>();

            using (var cursor = _context.Rides.FindAsync(filter).Result)
            {
                while (cursor.MoveNextAsync().Result)
                {
                    var batch = cursor.Current;
                    if (batch != null)
                        rides.AddRange(batch.Select(document => BsonSerializer.Deserialize<Ride>(document)));
                }
            }

            return rides;
        }

        public void Delete(string rideId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("RideId", rideId);

            _context.Rides.DeleteOneAsync(filter).GetAwaiter().GetResult();
        }

        public void DeleteRider(string rideId, string riderId)
        {
            throw new NotImplementedException();
        }
    }
}