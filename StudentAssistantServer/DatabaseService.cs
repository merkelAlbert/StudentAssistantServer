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
using StudentAssistantServer.DatabaseObjects;

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


        public async void PassHomework(List<HomeworkItem> homeworkItems)
        {
            var collection = Db.GetCollection<BsonDocument>("Homework");
            var models = new List<WriteModel<BsonDocument>>();
            foreach (var item in homeworkItems)
            {
                var id = new ObjectId(item.Id);
                var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
                models.Add(new ReplaceOneModel<BsonDocument>(filter, item.ToBsonDocument()));
            }

            await collection.BulkWriteAsync(models);
        }

        public async void DeleteHomework(List<string> ids)
        {
            var collection = Db.GetCollection<BsonDocument>("Homework");
            var models = new List<WriteModel<BsonDocument>>();
            foreach (var item in ids)
            {
                var id = new ObjectId(item);
                var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
                models.Add(new DeleteOneModel<BsonDocument>(filter));
            }

            await collection.BulkWriteAsync(models);
        }


        public async void ClearHomeworks(string userId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("userId", userId);
            var collection = Db.GetCollection<BsonDocument>("Homework");
            await collection.DeleteManyAsync(filter);
        }

        public async void ChangeUserInfo(UserInfoItem userInfoItem)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("userId", userInfoItem.UserId);
            var collection = Db.GetCollection<BsonDocument>("UsersInfo");
            await collection.ReplaceOneAsync(filter, userInfoItem.ToBsonDocument());
        }

        public async void ChangeHomework(HomeworkItem homeworkItem)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(homeworkItem.Id));
            var collection = Db.GetCollection<BsonDocument>("Homework");
            await collection.ReplaceOneAsync(filter, homeworkItem.ToBsonDocument());
        }

        public async void ChangeSchedule(ScheduleItem scheduleItem)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(scheduleItem.Id));
            var collection = Db.GetCollection<BsonDocument>("Schedule");
            await collection.ReplaceOneAsync(filter, scheduleItem.ToBsonDocument());
        }

        public async void ClearSchedule(string userId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("userId", userId);
            var collection = Db.GetCollection<BsonDocument>("Schedule");
            await collection.DeleteOneAsync(filter);
        }

        public async void ClearUserInfo(string userId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("userId", userId);
            var collection = Db.GetCollection<BsonDocument>("UsersInfo");
            await collection.DeleteOneAsync(filter);
        }

        public void ClearData(string userId)
        {
           ClearUserInfo(userId);
            ClearSchedule(userId);
            ClearHomeworks(userId);
        }
        
        
        public async void Add<T>(String collectionName, T item) where T : IDatabaseItem
        {
            var collection = Db.GetCollection<BsonDocument>(collectionName);
            item.Id = ObjectId.GenerateNewId().ToString();
            await collection.InsertOneAsync(item.ToBsonDocument());
        }

        public async void RemoveAccount(string userId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(userId));
            var collection = Db.GetCollection<BsonDocument>("Users");
            await collection.DeleteOneAsync(filter);
            ClearData(userId);
        }
    }
}