using System;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StudentAssistantServer.DatabaseObjects;

namespace StudentAssistantServer.Controllers
{
    public class TeachersController
    {
        private DatabaseService _databaseService;

        public TeachersController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpPost]
        [Route("changeTeachers/")]
        public JsonResult ChangeTeachers([FromBody] TeachersItem teachersItem)
        {
            if (teachersItem != null)
            {
                var filter = Builders<TeachersItem>.Filter.Eq("userId", teachersItem.UserId);
                var teachers = _databaseService.GetItemsByFilter("Teachers", filter);
                if (teachers == null)
                {
                    try
                    {
                        _databaseService.Add("Teachers", teachersItem);
                    }
                    catch (Exception e)
                    {
                        return new JsonResult(new
                        {
                            message = "Ошибка при добавлении информации о преподавателях!" +
                                      " Попробуйте позже",
                            status = 502
                        });
                    }

                    return new JsonResult(new
                    {
                        message = "Информация успешно добавлена",
                        status = 200
                    });
                }
                else
                {
                    try
                    {
                        _databaseService.ChangeTeachers(teachersItem);
                    }
                    catch (Exception e)
                    {
                        return new JsonResult(new
                        {
                            message = "Ошибка при изменении информации о пользователе! " +
                                      "Попробуйте позже",
                            status = 502
                        });
                    }

                    return new JsonResult(new
                    {
                        message = "Информация успешно изменена",
                        status = 200
                    });
                }
            }

            return new JsonResult(new
            {
                status = 404
            });
        }
    }
}