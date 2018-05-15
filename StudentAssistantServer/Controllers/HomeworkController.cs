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

        [HttpGet]
        [Route("homework/{userId}")]
        public JsonResult Homework([FromRoute] string userId)
        {
            //Console.WriteLine(Utils.GetWeekNumber(new DateTime(2018, 2, 7)));
            var filter = Builders<HomeworkItem>.Filter.Eq("userId", userId);
            var result = _databaseService.GetItemsByFilter("Homework", filter);
            if (result != null)
            {
                return new JsonResult(new
                {
                    homework = result,
                    status = 200
                });
            }
            else
            {
                return new JsonResult(new
                {
                    message="Домашние задания не найдены",
                    status = 517
                });
            }
        }

        [HttpPost]
        [Route("addHomework/")]
        public JsonResult AddHomework([FromBody] HomeworkItem homeworkItem)
        {
            try
            {
                _databaseService.Add("Homework", homeworkItem);
            }
            catch (Exception e)
            {
                return new JsonResult(new
                {
                    message = "Ошибка при добавлении домашнего задания!" +
                              " Попробуйте позже",
                    status = 502
                });
            }

            return new JsonResult(new
            {
                message = "Домашнее задание успешно добавлено",
                status = 200
            });
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
                return new JsonResult(new
                {
                    message = "Ошибка при изменении домашнего задания! " +
                              "Попробуйте позже",
                    status = 502
                });
            }

            return new JsonResult(new
            {
                message = "Домашнее задание успешно изменено",
                Status = 200
            });
        }
    }
}