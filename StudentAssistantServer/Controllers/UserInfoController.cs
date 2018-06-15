using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using StudentAssistantServer.DatabaseObjects;

namespace StudentAssistantServer.Controllers
{
    public class UserInfoController
    {
        private DatabaseService _databaseService;

        public UserInfoController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }


        [HttpGet]
        [Route("userInfo/{userId}")]
        public JsonResult UserInfo([FromRoute] string userId)
        {
            if (userId != null)
            {
                var iiFilter = Builders<UserInfoItem>.Filter.Eq("userId", userId);
                var userInfo = _databaseService.GetItemsByFilter("UsersInfo", iiFilter);

                var hwFilter = Builders<HomeworkItem>.Filter.Eq("userId", userId)
                               & Builders<HomeworkItem>.Filter.Eq("passed", false);

                var passedFilter = Builders<HomeworkItem>.Filter.Eq("userId", userId)
                                   & Builders<HomeworkItem>.Filter.Eq("passed", true);

                var passed = _databaseService.GetItemsByFilter("Homework", passedFilter) ?? new List<HomeworkItem>();
                
                var homework = _databaseService.GetItemsByFilter("Homework", hwFilter) ?? new List<HomeworkItem>();
                if (userInfo != null)
                {
                    return new JsonResult(new
                    {
                        userInfo = userInfo[0],
                        currentWeek = Utils.GetCurrentWeek(userInfo[0].StartDate),
                        totalHomework = homework.Count,
                        totalPassed = passed.Count,
                        status = 200
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        message = "Информация о пользователе не найдена",
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
        [Route("changeUser/")]
        public JsonResult ChangeUser([FromBody] UserInfoItem userInfoItem)
        {
            if (userInfoItem != null)
            {
                var filter = Builders<UserInfoItem>.Filter.Eq("userId", userInfoItem.UserId);
                var userInfo = _databaseService.GetItemsByFilter("UsersInfo", filter);
                if (userInfo == null)
                {
                    try
                    {
                        _databaseService.Add("UsersInfo", userInfoItem);
                    }
                    catch (Exception e)
                    {
                        return new JsonResult(new
                        {
                            message = "Ошибка при добавлении информации о пользователе!" +
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
                        _databaseService.ChangeUserInfo(userInfoItem);
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
        
        [HttpGet]
        [Route("clearData/{userId}")]
        public JsonResult ClearData([FromRoute] string userId)
        {
            if (userId != null)
            {
                try
                {
                    _databaseService.ClearData(userId);
                }
                catch (Exception e)
                {
                    return new JsonResult(new
                    {
                        message = "Ошибка при удалении данных! " +
                                  "Попробуйте позже",
                        status = 502
                    });
                }

                return new JsonResult(new
                {
                    message = "Данные успешно удалены",
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