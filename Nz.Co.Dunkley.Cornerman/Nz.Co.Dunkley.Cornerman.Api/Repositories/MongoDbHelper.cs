namespace Nz.Co.Dunkley.Cornerman.Api.Repositories
{
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class MongoDbHelper
    {
        private static IMongoClient _client;

        public static IMongoDatabase MongoCornermanDatabase() 
        {
            _client = new MongoClient();

            var database = _client.GetDatabase("Cornerman");

            var collection = database.GetCollection<BsonDocument>("Rides");

            var keys = Builders<BsonDocument>.IndexKeys.Ascending("RideId");

            collection.Indexes.CreateOneAsync(keys, new CreateIndexOptions() { Unique = true, });

            keys = Builders<BsonDocument>.IndexKeys.Ascending("Title");
            collection.Indexes.CreateOneAsync(keys, new CreateIndexOptions() { Unique = true, });

            return database;
        }
    }
}