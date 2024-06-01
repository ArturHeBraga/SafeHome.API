using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SafeHome.Entities
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("nome")]
        public string Nome { get; set; }

        [BsonElement("sobrenome")]
        public string Sobrenome { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("telefone")]
        public string Telefone { get; set; }

        [BsonElement("endereco")]
        public string Endereco { get; set; }
    }
}
