using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoListWebAPI.Models;
using TodoListWebAPI.Models.Settings;

namespace TodoListWebAPI.Services
{
    public class DataAccess
    {
        private readonly IConfiguration _configuration;
        private readonly MongoDbSettings _dbSettings;

        public DataAccess(IConfiguration configuration,
            IOptions<MongoDbSettings> options)
        {
            _configuration = configuration;
            _dbSettings = options.Value;
        }

        public IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(_dbSettings.ConnectionString);
            var db = client.GetDatabase(_dbSettings.DatabaseName);
            return db.GetCollection<T>(collection);
        }

        #region User

        public async Task<List<UserModel>> GetUser(string username)
        {
            var usersCollection = ConnectToMongo<UserModel>(_dbSettings.UserCollection);
            var results = await usersCollection.FindAsync(u => u.UserName == username);
            return results.ToList();
        }

        public Task CreateUser(UserModel user)
        {
            var usersCollection = ConnectToMongo<UserModel>(_dbSettings.UserCollection);
            return usersCollection.InsertOneAsync(user);
        }

        #endregion

        #region List

        public async Task<List<ListItemModel>> GetList(string username)
        {
            var listItemsCollection = ConnectToMongo<ListItemModel>(_dbSettings.ListItemCollection);
            var results = await listItemsCollection.FindAsync(li => li.UserName == username);
            return results.ToList();
        }

        public Task UpdateListItem(ListItemModel item)
        {
            var listItemsCollection = ConnectToMongo<ListItemModel>(_dbSettings.ListItemCollection);
            var filter = Builders<ListItemModel>.Filter.Eq("Id", item.Id);
            return listItemsCollection.ReplaceOneAsync(filter, item, new ReplaceOptions { IsUpsert = true });
        }

        public Task CreateListItem(ListItemModel item)
        {
            var listItemsCollection = ConnectToMongo<ListItemModel>(_dbSettings.ListItemCollection);
            return listItemsCollection.InsertOneAsync(item);
        }

        public Task DeleteListItem(ListItemModel item)
        {
            var listItemsCollection = ConnectToMongo<ListItemModel>(_dbSettings.ListItemCollection);
            var filter = Builders<ListItemModel>.Filter.Eq("Id", item.Id);
            return listItemsCollection.DeleteOneAsync(filter);
        }

        #endregion
    }
}
