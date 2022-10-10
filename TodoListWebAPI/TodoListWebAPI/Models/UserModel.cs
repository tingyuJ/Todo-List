using MongoDB.Bson;

namespace TodoListWebAPI.Models
{
    public class UserModel
    {
        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}