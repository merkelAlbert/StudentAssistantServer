using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudentAssistantServer
{
    public class UserInfoItem: IDatabaseItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("userId")] public string UserId { get; set; }
        [BsonElement("userName")] public string UserName { get; set; }
        [BsonElement("group")] public string Group { get; set; }
        [BsonElement("startDate")] public string StartDate { get; set; }
    }
}