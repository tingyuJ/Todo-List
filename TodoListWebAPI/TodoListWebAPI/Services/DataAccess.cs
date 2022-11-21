using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoListWebAPI.Models;
using TodoListWebAPI.Common.Settings;

namespace TodoListWebAPI.Services
{
    public class DataAccess
    {
        private readonly MongoDbSettings _dbSettings;

        public DataAccess(IOptions<MongoDbSettings> options)
        {
            _dbSettings = options.Value;
        }

        private IMongoCollection<T> ConnectToMongo<T>()
        {
            var client = new MongoClient(_dbSettings.ConnectionString);
            var db = client.GetDatabase(_dbSettings.DatabaseName);

            var model = Activator.CreateInstance<T>();
            var collection = (string)model.GetType().GetProperty("CollectionName").GetValue(model, null);

            return db.GetCollection<T>(collection);
        }

        #region User

        public async Task<List<UserModel>> GetUser(string username)
        {
            var usersCollection = ConnectToMongo<UserModel>();
            var results = await usersCollection.FindAsync(u => u.UserName == username);
            return results.ToList();
        }

        public Task CreateUser(UserModel user)
        {
            var usersCollection = ConnectToMongo<UserModel>();
            return usersCollection.InsertOneAsync(user);
        }

        #endregion

        #region List

        public async Task<List<ListItemModel>> GetList(string username)
        {
            var listItemsCollection = ConnectToMongo<ListItemModel>();
            var results = await listItemsCollection.FindAsync(li => li.UserName == username);
            return results.ToList();
        }

        public Task UpdateListItem(ListItemModel item)
        {
            var listItemsCollection = ConnectToMongo<ListItemModel>();
            var filter = Builders<ListItemModel>.Filter.Eq("Id", item.Id);
            return listItemsCollection.ReplaceOneAsync(filter, item, new ReplaceOptions { IsUpsert = true });
        }

        public Task CreateListItem(ListItemModel item)
        {
            var listItemsCollection = ConnectToMongo<ListItemModel>();
            return listItemsCollection.InsertOneAsync(item);
        }

        public Task DeleteListItem(ListItemModel item)
        {
            var listItemsCollection = ConnectToMongo<ListItemModel>();
            var filter = Builders<ListItemModel>.Filter.Eq("Id", item.Id);
            return listItemsCollection.DeleteOneAsync(filter);
        }

        #endregion
    }
}
