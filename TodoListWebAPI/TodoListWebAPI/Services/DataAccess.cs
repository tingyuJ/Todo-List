using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoListWebAPI.Common.Attributes;
using TodoListWebAPI.Models;
using TodoListWebAPI.Common.Settings;
using TodoListWebAPI.Interfaces;

namespace TodoListWebAPI.Services
{
    internal sealed class DataAccess : IDataAccess
    {
        private readonly IMongoDatabase _db;

        public DataAccess(IOptions<MongoDbSettings> options)
        {
            var dbSettings = options.Value;
            _db = new MongoClient(dbSettings.ConnectionString).GetDatabase(dbSettings.DatabaseName);
        }

        private IMongoCollection<T> ConnectToMongo<T>()
        {
            var collectionName = (MongoCollectionAttribute)typeof(T).GetCustomAttributes(typeof(MongoCollectionAttribute), true).First();

            return _db.GetCollection<T>(collectionName.CollectionName);
        }

        #region User

        public async Task<UserModel> GetUser(string username)
        {
            var usersCollection = ConnectToMongo<UserModel>();
            var results = await usersCollection.Find(u => u.UserName == username).Limit(1).FirstOrDefaultAsync();
            return results;
        }

        public Task CreateUser(UserModel user)
        {
            var usersCollection = ConnectToMongo<UserModel>();
            return usersCollection.InsertOneAsync(user);
        }

        #endregion

        #region List

        public async Task<IEnumerable<ListItemModel>> GetList(string username)
        {
            var listItemsCollection = ConnectToMongo<ListItemModel>();
            var results = await listItemsCollection.FindAsync(li => li.UserName == username);
            return results.ToEnumerable();
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
