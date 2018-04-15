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
        [Route("Users/{name}")]
        public JsonResult Users([FromRoute] string name)
        {
            _databaseService.Collection = _databaseService.Db.GetCollection<BsonDocument>("Users");
            return Json(_databaseService.GetDocumentsByFilter(Builders<BsonDocument>.Filter.Eq("Name", name)));
        }
    }
}