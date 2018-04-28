using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace StudentAssistantServer.Controllers
{
    public class ScheduleController : Controller
    {
        private DatabaseService _databaseService;

        public ScheduleController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // GET
        [Route("Schedule/")]
        public JsonResult Schedule()
        {
            //Console.WriteLine(Json(_databaseService.GetScheduleByFilter(new BsonDocument())));
            return Json(_databaseService.GetScheduleByFilter(new BsonDocument()));
        }
    }
}