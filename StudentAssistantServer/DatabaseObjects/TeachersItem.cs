using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudentAssistantServer.DatabaseObjects
{
    public class TeachersItem: IDatabaseItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("userId")] public string UserId { get; set; }

        [BsonElement("subjects")] public List<string> Subjects { get; set; }
        [BsonElement("teachers")] public List<string> Teachers { get; set; }
    }
}