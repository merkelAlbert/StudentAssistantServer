using System;
using System.Collections.Generic;
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
            if (userId != null)
            {
                //Console.WriteLine(Utils.GetWeekNumber(new DateTime(2018, 2, 7)));
                var homeworkFilter = Builders<HomeworkItem>.Filter.Eq("userId", userId);
                var userFilter = Builders<UserInfoItem>.Filter.Eq("userId", userId);
                var scheduleFilter = Builders<ScheduleItem>.Filter.Eq("userId", userId);

                var homeworkResult = _databaseService.GetItemsByFilter("Homework", homeworkFilter);
                var userResult = _databaseService.GetItemsByFilter("UsersInfo", userFilter);
                var scheduleResult = _databaseService.GetItemsByFilter("Schedule", scheduleFilter);

                if (scheduleResult == null)
                {
                    scheduleResult = new List<ScheduleItem>();
                }

                if (userResult == null)
                {
                    return new JsonResult(new
                    {
                        message = "Заполните информацию о пользователе",
                        status = 404
                    });
                }

                if (homeworkResult != null)
                {
                    var remainedList = new List<int>();
                    foreach (var homework in homeworkResult)
                    {
                        remainedList.Add(Utils.GetRemainedDays(userResult[0], homework, scheduleResult[0]));
                    }

                    return new JsonResult(new
                    {
                        homework = homeworkResult,
                        remainedDays = remainedList,
                        status = 200
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        message = "Домашние задания не найдены",
                        status = 404
                    });
                }
            }

            return new JsonResult(new
            {
                status = 404
            });
        }

        [HttpPost]
        [Route("addHomework/")]
        public JsonResult AddHomework([FromBody] HomeworkItem homeworkItem)
        {
            if (homeworkItem != null)
            {
                var scheduleFilter = Builders<ScheduleItem>.Filter.Eq("userId", homeworkItem.UserId);
                var userFilter = Builders<UserInfoItem>.Filter.Eq("userId", homeworkItem.UserId);

                var userResult = _databaseService.GetItemsByFilter("UsersInfo", userFilter);
                var scheduleResult = _databaseService.GetItemsByFilter("Schedule", scheduleFilter);


                if (userResult == null)
                {
                    return new JsonResult(new
                    {
                        message = "Заполните информацию о пользователе",
                        status = 404
                    });
                }

                try
                {
                    if (scheduleResult != null)
                    {
                        if (Utils.IsSubjectExistInWeek(homeworkItem, scheduleResult[0]))
                        {
                            _databaseService.Add("Homework", homeworkItem);
                        }
                        else
                        {
                            return new JsonResult(new
                            {
                                message = "На выбранной неделе нет выбранного предмета!",
                                status = 404
                            });
                        }
                    }
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

            return new JsonResult(new
            {
                status = 404
            });
        }

        [HttpPost]
        [Route("changeHomework/")]
        public JsonResult ChangeHomework([FromBody] HomeworkItem homeworkItem)
        {
            if (homeworkItem != null)
            {
                var scheduleFilter = Builders<ScheduleItem>.Filter.Eq("userId", homeworkItem.UserId);
                var scheduleResult = _databaseService.GetItemsByFilter("Schedule", scheduleFilter);
                try
                {
                    if (scheduleResult != null)
                    {
                        if (Utils.IsSubjectExistInWeek(homeworkItem, scheduleResult[0]))
                        {
                            _databaseService.ChangeHomework(homeworkItem);
                        }
                        else
                        {
                            return new JsonResult(new
                            {
                                message = "На выбранной неделе нет выбранного предмета!",
                                status = 404
                            });
                        }
                    }
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

            return new JsonResult(new
            {
                status = 404
            });
        }
        
        
        [HttpPost]
        [Route("changeHomework/")]
        public void PassHomework(List<HomeworkItem> homeworkItems)
        {
            
        }

    }
}