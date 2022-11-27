using MongoDB.Bson;
using MongoDB.Driver;
using TodoListWebAPI.Common.Attributes;

namespace TodoListWebAPI.Models
{
    [MongoCollection("users")]
    public class UserModel
    {
        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}