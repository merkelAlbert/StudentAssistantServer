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

        [HttpGet]
        [Route("schedule/")]
        public JsonResult Schedule()
        {
            //Console.WriteLine(Json(_databaseService.GetScheduleByFilter(new BsonDocument())));
            return Json(_databaseService.GetScheduleByFilter(new BsonDocument()));
        }
        
        [HttpPost]
        [Route("addSchedule/")]
        public JsonResult AddSchedule([FromBody] ScheduleItem scheduleItem)
        {
            _databaseService.AddSchedule(scheduleItem);
            return Json(null);
        }
    }
}