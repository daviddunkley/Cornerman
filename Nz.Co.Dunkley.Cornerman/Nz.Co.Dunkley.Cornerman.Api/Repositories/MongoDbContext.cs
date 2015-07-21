namespace Nz.Co.Dunkley.Cornerman.Api.Repositories
{
    using System;
    using Microsoft.Azure;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class MongoDbContext : IDisposable
    {
        private IMongoClient _client;
        private readonly IMongoDatabase _database;


        public MongoDbContext()
        {
            var connectionString = CloudConfigurationManager.GetSetting("MongoDb.ConnectionString");
            MongoUrl url = new MongoUrl(connectionString);
            _client = new MongoClient(url);
            _database = _client.GetDatabase("MongoLab-i");

            CreateRidesIndexes();
        }

        public IMongoDatabase Database
        {
            get
            {
                return _client.GetDatabase("MongoLab-i");
            }
        }

        public IMongoCollection<BsonDocument> Rides
        {
            get
            {
                return _database.GetCollection<BsonDocument>("Rides");
            }
        }

        private void CreateRidesIndexes()
        {
            var collection = _database.GetCollection<BsonDocument>("Rides");

            var keys = Builders<BsonDocument>.IndexKeys.Ascending("RideId");
            collection.Indexes.CreateOneAsync(
                keys, 
                new CreateIndexOptions()
                {
                    Unique = true,
                });

            keys = Builders<BsonDocument>.IndexKeys.Ascending("Lead.MembershipId");
            collection.Indexes.CreateOneAsync(keys);

            keys = Builders<BsonDocument>.IndexKeys.Ascending("Tailgunner.MembershipId");
            collection.Indexes.CreateOneAsync(keys);

            keys = Builders<BsonDocument>.IndexKeys.Ascending("Wingmen.MembershipId");
            collection.Indexes.CreateOneAsync(keys);

            keys = Builders<BsonDocument>.IndexKeys.Ascending("Riders.MembershipId");
            collection.Indexes.CreateOneAsync(keys);            
        }

        public void Dispose()
        {
            _client = null;
        }
    }
}