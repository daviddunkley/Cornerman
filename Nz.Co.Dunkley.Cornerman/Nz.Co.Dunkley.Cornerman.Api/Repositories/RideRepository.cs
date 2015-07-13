namespace Nz.Co.Dunkley.Cornerman.Api.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Models;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;

    public class RideRepository : IRideRepository
    {
        public async Task Create(Ride ride)
        {
            var rideDocument = ride.ToBsonDocument();

            var collection = MongoDbHelper
                .MongoCornermanDatabase()
                .GetCollection<BsonDocument>("Rides");

            await collection.InsertOneAsync(rideDocument);

            var keys = Builders<BsonDocument>.IndexKeys.Ascending("RideId");
            await collection.Indexes.CreateOneAsync(keys);

            keys = Builders<BsonDocument>.IndexKeys.Ascending("Title");
            await collection.Indexes.CreateOneAsync(keys);

        }

        public async Task<Ride> Retrieve(string rideId)
        {
            var collection = MongoDbHelper
                .MongoCornermanDatabase()
                .GetCollection<BsonDocument>("Rides");

            var filter = Builders<BsonDocument>.Filter.Eq("RideId", rideId);

            Ride ride = null;

            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
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

        public async Task<List<Ride>> RetrieveForMembershipId(string membershipId)
        {
            var collection = MongoDbHelper
                .MongoCornermanDatabase()
                .GetCollection<BsonDocument>("Rides");

            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("Lead.MembershipId", membershipId);

            List<Ride> rides = new List<Ride>();

            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    if (batch != null)
                        rides.AddRange(batch.Select(document => BsonSerializer.Deserialize<Ride>(document)));
                }
            }

            return rides;
        }

        public Task Update(Ride ride)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string rideId)
        {
            var collection = MongoDbHelper
                .MongoCornermanDatabase()
                .GetCollection<BsonDocument>("Rides");
            var filter = Builders<BsonDocument>.Filter.Eq("RideId", rideId);

            await collection.DeleteManyAsync(filter);
        }
    }
}