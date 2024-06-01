using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SafeHome.Entities
{
    public class Sensor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("gas")]
        public string Gas { get; set; }

        [BsonElement("temp")]
        public string Temperatura { get; set; }


    }
}
