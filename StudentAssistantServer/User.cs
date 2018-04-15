using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudentAssistantServer
{
    public class User
    {
        [BsonId][BsonRepresentation(BsonType.ObjectId)] public string Id { get; set; }
        [BsonElement("Name")] public string Name { get; set; }
    }
}