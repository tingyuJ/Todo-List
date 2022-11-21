using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using MongoDB.Driver;
using NUnit.Framework;
using TodoListWebAPI.Services;
using TodoListWebAPI.Models;
using TodoListWebAPI.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore;

namespace TodoListWepAPITest
{
    [TestFixture]
    public class Tests
    {
        private IConfiguration _config;
        static IWebHost _webHost;
        private MongoDbSettings _dbSettings;
        private DataAccess _db;

        #region Utils

        private void Init()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .AddEnvironmentVariables()
                .Build();

            _webHost = WebHost.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<DataAccess>();
                    services.AddSingleton<JwtGenerator>();
                    services.Configure<MongoDbSettings>(_config.GetSection(MongoDbSettings.DbSettings));
                })
                .Configure(app => { })
                .Build();

            _dbSettings = _config.GetSection(MongoDbSettings.DbSettings).Get<MongoDbSettings>();
            _db = GetService<DataAccess>();
        }

        private T GetService<T>()
        {
            var scope = _webHost.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<T>();
        }

        private IMongoCollection<T> ConnectToMongo<T>()
        {
            var client = new MongoClient(_dbSettings.ConnectionString);
            var db = client.GetDatabase(_dbSettings.DatabaseName);

            var model = Activator.CreateInstance<T>();
            var collection = (string)model.GetType().GetProperty("CollectionName").GetValue(model, null);

            return db.GetCollection<T>(collection);
        }

        #endregion

        #region SetUp / TearDown

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Init();
        }

        [SetUp]
        public async Task SetUp()
        {
            var usersCollection = ConnectToMongo<UserModel>();
            await usersCollection.InsertOneAsync(new UserModel() { UserName = "test999", Password = "test999" });

            var listItemCollection = ConnectToMongo<ListItemModel>();
            await listItemCollection.InsertManyAsync(new List<ListItemModel>()
            {
                new() { UserName = "test999", Checked = false, Text = "test999-1" },
                new() { UserName = "test999", Checked = false, Text = "test999-2" },
                new() { UserName = "test999", Checked = false, Text = "test999-3" },
            });
        }

        [TearDown]
        public async Task TearDown()
        {
            var usersCollection = ConnectToMongo<UserModel>();
            await usersCollection.DeleteOneAsync(Builders<UserModel>.Filter.Eq("UserName", "test999"));

            var listItemCollection = ConnectToMongo<ListItemModel>();
            await listItemCollection.DeleteManyAsync(Builders<ListItemModel>.Filter.Eq("UserName", "test999"));
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            //delete the created users
            var usersCollection = ConnectToMongo<UserModel>();
            await usersCollection.DeleteOneAsync(Builders<UserModel>.Filter.Eq("UserName", "test30"));
        }

        #endregion

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }


        #region User

        [Test]
        [TestCase("test999", 1, TestName = "Get test user 999")]
        [TestCase("test100", 0, TestName = "Get test user 100")]
        public async Task GetUser(string username, int count)
        {
            var result = await _db.GetUser(username);

            Assert.AreEqual(count, result.Count);
        }

        [Test]
        [TestCase(TestName = "Create test user 30")]
        public async Task CreateUser()
        {
            var user = new UserModel()
            {
                UserName = "test30",
                Password = "test30"
            };

            var before = await _db.GetUser("test30");
            Assert.AreEqual(0, before.Count);

            await _db.CreateUser(user);

            var after = await _db.GetUser("test30");
            Assert.AreEqual(1, after.Count);
        }

        #endregion

        #region List

        [Test]
        [TestCase("test999", 3, TestName = "Get test999 user todo list")]
        public async Task GetList(string username, int itemCount)
        {
            var list = await _db.GetList(username);

            Assert.AreEqual(itemCount, list.Count);
        }

        [Test]
        [TestCase("test999", TestName = "Check unchecked list items")]
        public async Task CheckUncheckedListItems(string username)
        {
            var listItemCollection = ConnectToMongo<ListItemModel>();
            var uncheckedBefore = await listItemCollection.FindAsync(li => li.Checked == false && li.UserName == username);

            uncheckedBefore?.ToList().ForEach(li =>
            {
                li.Checked = true;
                _db.UpdateListItem(li);
            });

            var uncheckedAfter = await listItemCollection.FindAsync(li => li.Checked == false && li.UserName == username);
            Assert.AreEqual(0, uncheckedAfter.ToList().Count);
        }

        [Test]
        [TestCase("test999", 1, 4, TestName = "Create 1 list item")]
        [TestCase("test999", 5, 8, TestName = "Create 5 list items")]
        public async Task CreateListItem(string username, int createAmount, int expectedCount)
        {
            for (int i = 0; i < createAmount; i++)
            {
                await _db.CreateListItem(new ListItemModel()
                    { UserName = username, Checked = false, Text = $"{username}-{i}" });
            }

            var listItemCollection = ConnectToMongo<ListItemModel>();
            var list = await listItemCollection.FindAsync(li => li.UserName == username);

            Assert.AreEqual(expectedCount, list.ToList().Count);
        }

        [Test]
        [TestCase("test999", 1, 2, TestName = "Delete 1 list item")]
        [TestCase("test999", 3, 0, TestName = "Delete 3 list items")]
        public async Task DeleteListItem(string username, int deleteAmount, int countAfter)
        {
            var listItemCollection = ConnectToMongo<ListItemModel>();
            var listBefore = await listItemCollection.FindAsync(li => li.UserName == username).Result.ToListAsync();

            for (int i = 0; i < deleteAmount; i++)
            {
                await _db.DeleteListItem(listBefore[i]);
            }
            
            var listAfter = await listItemCollection.FindAsync(li => li.UserName == username).Result.ToListAsync();
            Assert.AreEqual(countAfter, listAfter.Count);
        }

        [Test]
        [TestCase("test999", "test999-2", TestName = "Delete test999-2")]
        public async Task DeleteSpecificListItem(string username, string itemText)
        {
            var listItemCollection = ConnectToMongo<ListItemModel>();
            var listBefore = await listItemCollection.FindAsync(li => li.UserName == username && li.Text == itemText).Result.ToListAsync();
            Assert.AreEqual(1, listBefore.ToList().Count);

            await _db.DeleteListItem(listBefore.FirstOrDefault());
            
            var listAfter = await listItemCollection.FindAsync(li => li.UserName == username && li.Text == itemText).Result.ToListAsync();
            Assert.AreEqual(0, listAfter.Count);
        }

        #endregion
        
    }
}