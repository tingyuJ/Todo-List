using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TodoListWebAPI.Common.Attributes;

namespace TodoListWebAPI.Models
{
    [MongoCollection("list_items")]
    public class ListItemModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool Checked { get; set; }
        public string Text { get; set; }
    }
}