using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudentAssistantServer
{
    public class HomeworkItem
    {
        [BsonId][BsonRepresentation(BsonType.ObjectId)] public string Id { get; set; }
        [BsonElement("subject")] public string Subject { get; set; }
        [BsonElement("exercise")] public string Exercise { get; set; }
        [BsonElement("time")] public int Time { get; set; }
        
    }
    
}