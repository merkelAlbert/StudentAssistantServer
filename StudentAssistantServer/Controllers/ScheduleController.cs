using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using StudentAssistantServer.DatabaseObjects;

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
        [Route("schedule/{userId}")]
        public JsonResult Schedule([FromRoute] string userId)
        {
            if (userId != null)
            {
                var filter = Builders<ScheduleItem>.Filter.Eq("userId", userId);
                if (_databaseService.GetItemsByFilter<ScheduleItem>("Schedule", filter) != null)
                {
                    var schedule = _databaseService.GetItemsByFilter<ScheduleItem>("Schedule", filter)[0];
                    return new JsonResult(
                        new
                        {
                            schedule = schedule,
                            subjects = Utils.GetSubjects(schedule),
                            status = 200
                        });
                }
                else
                {
                    return new JsonResult(new
                    {
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
        [Route("addSchedule/")]
        public JsonResult AddSchedule([FromBody] ScheduleItem scheduleItem)
        {
            if (scheduleItem != null)
            {
                try
                {
                    _databaseService.Add("Schedule", scheduleItem);
                }
                catch (Exception e)
                {
                    return new JsonResult(new
                    {
                        message = "Ошибка при добавлении расписания!" +
                                  " Попробуйте позже",
                        status = 502
                    });
                }

                return new JsonResult(new
                {
                    message = "Расписание успешно добавлено",
                    status = 200
                });
            }

            return new JsonResult(new
            {
                status = 404
            });
        }

        [HttpPost]
        [Route("changeSchedule/")]
        public JsonResult ChangeSchedule([FromBody] ScheduleItem scheduleItem)
        {
            if (scheduleItem != null)
            {
                try
                {
                    _databaseService.ChangeSchedule(scheduleItem);
                }
                catch (Exception e)
                {
                    return new JsonResult(new
                    {
                        message = "Ошибка при изменении расписания! " +
                                  "Попробуйте позже",
                        status = 502
                    });
                }

                return new JsonResult(new
                {
                    message = "Расписание успешно изменено",
                    Status = 200
                });
            }

            return new JsonResult(new
            {
                status = 404
            });
        }

        [HttpGet]
        [Route("clearSchedule/{userId}")]
        public JsonResult ClearSchedule([FromRoute] string userId)
        {
            if (userId != null)
            {
                try
                {
                    _databaseService.ClearSchedule(userId);
                }
                catch (Exception e)
                {
                    return new JsonResult(new
                    {
                        message = "Ошибка при удалении расписания! " +
                                  "Попробуйте позже",
                        status = 502
                    });
                }

                return new JsonResult(new
                {
                    message = "Расписание успешно удалено",
                    Status = 200
                });
            }

            return new JsonResult(new
            {
                status = 404
            });
        }
    }
}