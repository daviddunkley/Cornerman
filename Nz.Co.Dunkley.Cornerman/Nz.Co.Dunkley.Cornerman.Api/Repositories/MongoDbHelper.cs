namespace Nz.Co.Dunkley.Cornerman.Api.Repositories
{
    using MongoDB.Driver;

    public class MongoDbHelper
    {
        private static IMongoClient _client;

        public static IMongoDatabase MongoCornermanDatabase() 
        {
            _client = new MongoClient();

            return _client.GetDatabase("Cornerman");
        }
    }
}