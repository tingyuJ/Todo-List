namespace TodoListWebAPI.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MongoCollectionAttribute : Attribute
    {
        public MongoCollectionAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
        public string CollectionName { get; }
    }
}
