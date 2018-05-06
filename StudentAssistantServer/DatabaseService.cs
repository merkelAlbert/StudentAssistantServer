using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public List<UserItem> GetDocumentsByFilter(FilterDefinition<UserItem> filter)
        {
            var collection = Db.GetCollection<UserItem>("User");
            var users = collection.Find(filter).ToList();
            return users;
        }

        public List<HomeworkItem> GetHomeworkByFilter(FilterDefinition<HomeworkItem> filter)
        {
            var collection = Db.GetCollection<HomeworkItem>("Homework");
            var homeworks = collection.Find(filter).ToList();
            return homeworks;
        }

        public ScheduleItem GetScheduleByFilter(FilterDefinition<ScheduleItem> filter)
        {
            var collection = Db.GetCollection<ScheduleItem>("Schedule");
            var schedules = collection.Find(filter).ToList();

            return schedules[0];
        }

        public async void AddHomework(HomeworkItem homeworkItem)
        {
            var collection = Db.GetCollection<BsonDocument>("Homework");
            homeworkItem.Id = ObjectId.GenerateNewId().ToString();
            await collection.InsertOneAsync(homeworkItem.ToBsonDocument());
        }

        public async void AddSchedule(ScheduleItem scheduleItem)
        {
            var collection = Db.GetCollection<BsonDocument>("Schedule");
            scheduleItem.Id = ObjectId.GenerateNewId().ToString();
            await collection.InsertOneAsync(scheduleItem.ToBsonDocument());
        }

        public async Task ChangeHomework(HomeworkItem homeworkItem)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(homeworkItem.Id));
            var collection = Db.GetCollection<BsonDocument>("Homework");
            await collection.ReplaceOneAsync(filter, homeworkItem.ToBsonDocument());
        }
    }
}