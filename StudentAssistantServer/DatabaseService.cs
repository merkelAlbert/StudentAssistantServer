using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace StudentAssistantServer
{
    public class DatabaseService
    {
        public string Name { get; }
        public string ConnectionString { get; }
        public IMongoDatabase Db { get; private set; }

        public DatabaseService()
        {
            Name = Settings.Database;
            ConnectionString = Settings.ConnectionString;
            var client = new MongoClient(ConnectionString);
            Db = client.GetDatabase(Name);
        }

        public List<T> GetItemsByFilter<T>(string collectionName, FilterDefinition<T> filter)
        {
            var collection = Db.GetCollection<T>(collectionName);
            var items = collection.Find(filter).ToList();
            if (items.Count > 0)
                return items;
            return null;
        }


        public async void ChangeHomework(HomeworkItem homeworkItem)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(homeworkItem.Id));
            var collection = Db.GetCollection<BsonDocument>("Homework");
            await collection.ReplaceOneAsync(filter, homeworkItem.ToBsonDocument());
        }

        public async void Add<T>(String collectionName, T item) where T : IDatabaseItem
        {
            var collection = Db.GetCollection<BsonDocument>(collectionName);
            item.Id = ObjectId.GenerateNewId().ToString();
            await collection.InsertOneAsync(item.ToBsonDocument());
        }
    }
}