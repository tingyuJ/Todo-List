namespace TodoListWebAPI.Common.Settings
{
    public class MongoDbSettings
    {
        public const string DbSettings = "MongoDbSettings";

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
        
        public string UserCollection { get; set; }

        public string ListItemCollection { get; set; }
    }
}
