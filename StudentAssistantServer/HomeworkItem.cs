using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudentAssistantServer
{
    public class HomeworkItem : IDatabaseItem
    {
        [BsonId][BsonRepresentation(BsonType.ObjectId)] public string Id { get; set; }
        [BsonElement("userId")] public string UserId { get; set; }
        [BsonElement("subject")] public string Subject { get; set; }
        [BsonElement("exercise")] public string Exercise { get; set; }
        [BsonElement("week")] public int Week { get; set; }
    }
    
}