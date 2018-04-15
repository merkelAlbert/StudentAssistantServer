using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace StudentAssistantServer
{
    public class DatabaseService
    {
        public string Name { get; }
        public string ConnectionString { get; }
        public IMongoDatabase Db { get; private set; }
        public IMongoCollection<BsonDocument> Collection { get; set; }

        public DatabaseService()
        {
            Name = Settings.Database;
            ConnectionString = Settings.ConnectionString;
            var client = new MongoClient(ConnectionString);
            Db = client.GetDatabase(Name);
        }

        public List<User> GetDocumentsByFilter(FilterDefinition<BsonDocument> filter)
        {
            var users = Collection.Find(filter).ToList();
            List<User> myList = new List<User>();
            foreach (var user in users)
            {
                
                myList.Add(BsonSerializer.Deserialize<User>(user));

            }

            return myList;
        }
    }
}