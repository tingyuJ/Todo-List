using MongoDB.Driver;
using TodoListWebAPI.Models;

namespace TodoListWebAPI.Services
{
    public class DataAccess
    {
        private const string ConnectionString = "mongodb://127.0.0.1:27017";
        private const string DatabaseName = "todo_list";
        private const string UserCollection = "users";
        private const string ListItemCollection = "list_items";

        private IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DatabaseName);
            return db.GetCollection<T>(collection);
        }

        #region User

        public async Task<List<UserModel>> GetUser(string username)
        {
            var usersCollection = ConnectToMongo<UserModel>(UserCollection);
            var results = await usersCollection.FindAsync(u => u.UserName == username);
            return results.ToList();
        }

        public Task CreateUser(UserModel user)
        {
            var usersCollection = ConnectToMongo<UserModel>(UserCollection);
            return usersCollection.InsertOneAsync(user);
        }

        #endregion

        #region List

        public async Task<List<ListItemModel>> GetList(string username)
        {
            var listItemsCollection = ConnectToMongo<ListItemModel>(ListItemCollection);
            var results = await listItemsCollection.FindAsync(li => li.UserName == username);
            return results.ToList();
        }

        public Task UpdateListItem(ListItemModel item)
        {
            var listItemsCollection = ConnectToMongo<ListItemModel>(ListItemCollection);
            var filter = Builders<ListItemModel>.Filter.Eq("Id", item.Id);
            return listItemsCollection.ReplaceOneAsync(filter, item, new ReplaceOptions { IsUpsert = true });
        }

        public Task CreateListItem(ListItemModel item)
        {
            var listItemsCollection = ConnectToMongo<ListItemModel>(ListItemCollection);
            return listItemsCollection.InsertOneAsync(item);
        }

        public Task DeleteListItem(ListItemModel item)
        {
            var listItemsCollection = ConnectToMongo<ListItemModel>(ListItemCollection);
            var filter = Builders<ListItemModel>.Filter.Eq("Id", item.Id);
            return listItemsCollection.DeleteOneAsync(filter);
        }

        #endregion
    }
}
