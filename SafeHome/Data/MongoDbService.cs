using MongoDB.Bson;
using MongoDB.Driver;

namespace SafeHome.Data
{
    public class MongoDbService
    {
        public IMongoDatabase Database { get; }

        public MongoDbService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DbConnection");
            var mongoClient = new MongoClient(connectionString);
            Database = mongoClient.GetDatabase("user");
        }

        public void TestConnection()
        {
            try
            {
                var result = Database.RunCommand((Command<BsonDocument>)"{ping:1}");
                Console.WriteLine("MongoDB connection successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MongoDB connection error: {ex.Message}");
            }
        }
    }
}
