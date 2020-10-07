using ProfileBook.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProfileBook.Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly SQLiteConnection database;

        public RepositoryService()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            database = new SQLiteConnection(Path.Combine(path, "Users.db"));
            database.CreateTable<UserModel>();
        }

        public IEnumerable<UserModel> GetItems()
        {
            return database.Table<UserModel>().ToList();
        }

        public UserModel GetItem(int id)
        {
            return database.Get<UserModel>(id);
        }

        public int DeleteItem(int id)
        {
            return database.Delete<UserModel>(id);
        }

        public int SaveItem(UserModel item)
        {
            int result = -1;
            try
            {
                if (item.Id != 0)
                {
                    database.Update(item);
                    return item.Id;
                }
                else
                {
                    return database.Insert(item);
                }
            }
            catch
            {
            }

            return result;
        }
    }
}
