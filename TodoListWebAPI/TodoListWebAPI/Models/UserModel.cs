using MongoDB.Bson;
using MongoDB.Driver;

namespace TodoListWebAPI.Models
{
    public class UserModel
    {
        public string CollectionName => "UserCollection";

        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}