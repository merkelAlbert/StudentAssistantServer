using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudentAssistantServer.DatabaseObjects
{

    public class ScheduleItem : IDatabaseItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("userId")] public string UserId { get; set; }

        [BsonElement("schedule")] public List<List<List<string>>> Schedule { get; set; }

    }
}