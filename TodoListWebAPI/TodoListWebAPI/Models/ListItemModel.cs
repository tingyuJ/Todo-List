using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoListWebAPI.Models
{
    public class ListItemModel
    {
        public string CollectionName => "ListItemCollection";

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool Checked { get; set; }
        public string Text { get; set; }
    }
}