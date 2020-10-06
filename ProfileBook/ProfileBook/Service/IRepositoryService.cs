using ProfileBook.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProfileBook.Service
{
    public interface IRepositoryService
    {
        IEnumerable<UserModel> GetItems();
        UserModel GetItem(int id);
        int DeleteItem(int id);
        int SaveItem(UserModel item);
    }
}
