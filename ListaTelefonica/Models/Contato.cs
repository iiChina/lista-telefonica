using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace ListaTelefonica.Models;

public class Contato
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("nome")]
    [JsonPropertyName("nome")]
    public string Nome { get; set; }

    [BsonElement("sobrenome")]
    [JsonPropertyName("Sobrenome")]
    public string Sobrenome { get; set; }

    [BsonElement("telefone")]
    [JsonPropertyName("telefone")]
    public string Telefone { get; set; }

    [BsonElement("cpf")]
    [JsonPropertyName("cpf")]
    public string Cpf { get; set; }
}
