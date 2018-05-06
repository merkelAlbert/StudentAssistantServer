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
        [HttpGet]
        [Route("homework/")]
        public JsonResult Homework()
        {
            return Json(_databaseService.GetHomeworkByFilter(new BsonDocument()));
        }

        [HttpPost]
        [Route("addHomework/")]
        public JsonResult AddHomework([FromBody] HomeworkItem homeworkItem)
        {
            try
            {
                _databaseService.AddHomework(homeworkItem);
            }
            catch (Exception e)
            {
                return new JsonResult(new Response("Ошибка при добавлении домашнего задания! Попробуте позже"));
            }

            return new JsonResult(new Response("Домашнее задание успешно добавлено"));
        }
        
        [HttpPost]
        [Route("changeHomework/")]
        public JsonResult ChangeHomework([FromBody] HomeworkItem homeworkItem)
        {
            try
            {
                _databaseService.ChangeHomework(homeworkItem);
            }
            catch (Exception e)
            {
                return new JsonResult(new Response("Ошибка при изменении домашнего задания! Попробуте позже"));
            }

            return new JsonResult(new Response("Домашнее задание успешно изменено"));
        }
    }
}