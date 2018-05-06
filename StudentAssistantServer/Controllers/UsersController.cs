using System;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace StudentAssistantServer.Controllers
{
    public class UsersController : Controller
    {
        private DatabaseService _databaseService;

        public UsersController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        
        // GET
        [HttpGet]
        [Route("users/{name}")]
        public JsonResult Users([FromRoute] string name)
        {
            return Json(_databaseService.GetDocumentsByFilter(Builders<UserItem>.Filter.Eq("Name", name)));
        }
    }
}