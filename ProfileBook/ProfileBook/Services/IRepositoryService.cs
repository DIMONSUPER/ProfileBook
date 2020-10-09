using ProfileBook.Models;
using System.Collections.Generic;

namespace ProfileBook.Services
{
    public interface IRepositoryService
    {
        IEnumerable<UserModel> GetItems();
        UserModel GetItem(int id);
        int DeleteItem(int id);
        int SaveItem(UserModel item);
    }
}
