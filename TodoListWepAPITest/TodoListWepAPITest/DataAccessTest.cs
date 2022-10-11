using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using NUnit.Framework;
using TodoListWebAPI.Services;
using TodoListWebAPI.Models;

namespace TodoListWepAPITest
{
    public class Tests
    {
        private DataAccess db = new DataAccess();

        [SetUp]
        public async Task SetUp()
        {
            var usersCollection = db.ConnectToMongo<UserModel>(DataAccess.UserCollection);
            _ = usersCollection.InsertOneAsync(new UserModel() { UserName = "test999", Password = "test999" });

            var listItemCollection = db.ConnectToMongo<ListItemModel>(DataAccess.ListItemCollection);
            listItemCollection.InsertManyAsync(new List<ListItemModel>()
            {
                new() { UserName = "test999", Checked = false, Text = "test999-1" },
                new() { UserName = "test999", Checked = false, Text = "test999-2" },
                new() { UserName = "test999", Checked = false, Text = "test999-3" },
            });
        }

        [TearDown]
        public void TearDown()
        {
            var usersCollection = db.ConnectToMongo<UserModel>(DataAccess.UserCollection);
            usersCollection.DeleteOneAsync(Builders<UserModel>.Filter.Eq("UserName", "test999"));

            var listItemCollection = db.ConnectToMongo<ListItemModel>(DataAccess.ListItemCollection);
            listItemCollection.DeleteManyAsync(Builders<ListItemModel>.Filter.Eq("UserName", "test999"));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            //delete the created users
            var usersCollection = db.ConnectToMongo<UserModel>(DataAccess.UserCollection);
            usersCollection.DeleteOneAsync(Builders<UserModel>.Filter.Eq("UserName", "test30"));
        }

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
            var result = await db.GetUser(username);

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

            var before = await db.GetUser("test30");
            Assert.AreEqual(0, before.Count);

            await db.CreateUser(user);

            var after = await db.GetUser("test30");
            Assert.AreEqual(1, after.Count);
        }

        #endregion

        #region List

        [Test]
        [TestCase("test999", 3, TestName = "Get test999 user todo list")]
        public async Task GetList(string username, int itemCount)
        {
            var list = await db.GetList(username);

            Assert.AreEqual(itemCount, list.Count);
        }

        [Test]
        [TestCase("test999", TestName = "Check unchecked list items")]
        public async Task CheckUncheckedListItems(string username)
        {
            var listItemCollection = db.ConnectToMongo<ListItemModel>(DataAccess.ListItemCollection);
            var uncheckedBefore = await listItemCollection.FindAsync(li => li.Checked == false && li.UserName == username);

            uncheckedBefore?.ToList().ForEach(li =>
            {
                li.Checked = true;
                db.UpdateListItem(li);
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
                await db.CreateListItem(new ListItemModel()
                    { UserName = username, Checked = false, Text = $"{username}-{i}" });
            }

            var listItemCollection = db.ConnectToMongo<ListItemModel>(DataAccess.ListItemCollection);
            var list = await listItemCollection.FindAsync(li => li.UserName == username);

            Assert.AreEqual(expectedCount, list.ToList().Count);
        }

        [Test]
        [TestCase("test999", 1, 2, TestName = "Delete 1 list item")]
        [TestCase("test999", 3, 0, TestName = "Delete 3 list items")]
        public async Task DeleteListItem(string username, int deleteAmount, int countAfter)
        {
            var listItemCollection = db.ConnectToMongo<ListItemModel>(DataAccess.ListItemCollection);
            var listBefore = await listItemCollection.FindAsync(li => li.UserName == username).Result.ToListAsync();

            for (int i = 0; i < deleteAmount; i++)
            {
                await db.DeleteListItem(listBefore[i]);
            }
            
            var listAfter = await listItemCollection.FindAsync(li => li.UserName == username).Result.ToListAsync();
            Assert.AreEqual(countAfter, listAfter.Count);
        }

        [Test]
        [TestCase("test999", "test999-2", TestName = "Delete test999-2")]
        public async Task DeleteSpecificListItem(string username, string itemText)
        {
            var listItemCollection = db.ConnectToMongo<ListItemModel>(DataAccess.ListItemCollection);
            var listBefore = await listItemCollection.FindAsync(li => li.UserName == username && li.Text == itemText).Result.ToListAsync();
            Assert.AreEqual(1, listBefore.ToList().Count);

            await db.DeleteListItem(listBefore.FirstOrDefault());
            
            var listAfter = await listItemCollection.FindAsync(li => li.UserName == username && li.Text == itemText).Result.ToListAsync();
            Assert.AreEqual(0, listAfter.Count);
        }

        #endregion
    }
}