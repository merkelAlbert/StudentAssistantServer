using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}