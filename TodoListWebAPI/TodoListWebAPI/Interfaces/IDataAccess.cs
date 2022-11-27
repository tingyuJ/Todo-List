using TodoListWebAPI.Models;

namespace TodoListWebAPI.Interfaces;

public interface IDataAccess
{
    Task<UserModel> GetUser(string username);
    Task CreateUser(UserModel user);
    Task<IEnumerable<ListItemModel>> GetList(string username);
    Task UpdateListItem(ListItemModel item);
    Task CreateListItem(ListItemModel item);
    Task DeleteListItem(ListItemModel item);
}