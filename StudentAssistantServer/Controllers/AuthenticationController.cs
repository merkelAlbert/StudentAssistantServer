using System;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using StudentAssistantServer.DatabaseObjects;

namespace StudentAssistantServer.Controllers
{
    public class AuthenticationController : Controller
    {
        private DatabaseService _databaseService;

        public AuthenticationController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // GET
        [HttpPost]
        [Route("login/")]
        public JsonResult Login([FromBody] UserItem userItem)
        {
            if (userItem != null)
            {
                var filter = Builders<UserItem>.Filter.Eq("email", userItem.Email);
                var users = _databaseService.GetItemsByFilter("Users", filter);
                if (users == null)
                {
                    return new JsonResult(new
                    {
                        message = "Пользователь с данным email отсутствует",
                        status = 417
                    });
                }

                if (users[0].Password.Equals(userItem.Password))
                {
                    return new JsonResult(new
                    {
                        message = "Вход выполнен",
                        status = 200,
                        id = users[0].Id
                    });
                }

                return new JsonResult(new
                {
                    message = "Неверный пароль",
                    status = 417
                });
            }

            return new JsonResult(new
            {
                status = 404
            });
        }

        [HttpPost]
        [Route("register/")]
        public JsonResult Register([FromBody] UserItem userItem)
        {
            if (userItem != null)
            {
                var filter = Builders<UserItem>.Filter.Eq("email", userItem.Email);
                if (_databaseService.GetItemsByFilter("Users", filter) != null)
                {
                    return new JsonResult(new
                    {
                        message = "Пользователь с данным email уже существует",
                        status = 417
                    });
                }

                try
                {
                    _databaseService.Add("Users", userItem);
                }
                catch (Exception e)
                {
                    return new JsonResult(new
                    {
                        message = "Ошибка регистрации! Попробуйте позже",
                        status = 502
                    });
                }

                return new JsonResult(new
                {
                    message = "Регистрация прошла успешно",
                    status = 200
                });
            }

            return new JsonResult(new
            {
                status = 404
            });
        }
        
        [HttpGet]
        [Route("removeAccount/{userId}")]
        public JsonResult RemoveAccount([FromRoute] string userId)
        {
            if (userId != null)
            {
                try
                {
                    _databaseService.RemoveAccount(userId);
                }
                catch (Exception e)
                {
                    return new JsonResult(new
                    {
                        message = "Ошибка при удалении аккаунта! " +
                                  "Попробуйте позже",
                        status = 502
                    });
                }

                return new JsonResult(new
                {
                    message = "Аккаунт успешно удален",
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