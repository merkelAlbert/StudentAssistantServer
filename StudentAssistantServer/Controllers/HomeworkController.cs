using System;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace StudentAssistantServer.Controllers
{
    public class HomeworkController : Controller
    {
        private DatabaseService _databaseService;

        public HomeworkController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        
        // GET
        [Route("Homework/")]
        public JsonResult Homework()
        {
            return Json(_databaseService.GetHomeworkByFilter(new BsonDocument()));
        }
    }
}