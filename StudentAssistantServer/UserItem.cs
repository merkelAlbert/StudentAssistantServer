using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudentAssistantServer
{
    public class UserItem : IDatabaseItem
    {
        [BsonId][BsonRepresentation(BsonType.ObjectId)] public string Id { get; set; }
        [BsonElement("email")] public string Email { get; set; }
        [BsonElement("password")] public string Password { get; set; }
    }
}