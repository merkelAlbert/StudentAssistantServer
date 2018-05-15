using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudentAssistantServer
{
    public interface IDatabaseItem
    {
        [BsonId][BsonRepresentation(BsonType.ObjectId)] string Id { get; set; }
    }
}