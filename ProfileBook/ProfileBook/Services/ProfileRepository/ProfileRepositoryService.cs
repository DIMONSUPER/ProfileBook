using ProfileBook.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProfileBook.Services.ProfileRepository
{
    public class ProfileRepositoryService : IProfileRepositoryService
    {
        private readonly SQLiteConnection database;

        public ProfileRepositoryService()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            database = new SQLiteConnection(Path.Combine(path, "profiles_database.db"));
            database.CreateTable<ProfileModel>();
        }

        public IEnumerable<ProfileModel> GetItems()
        {
            return database.Table<ProfileModel>().ToList();
        }

        public ProfileModel GetItem(int id)
        {
            return database.Get<ProfileModel>(id);
        }

        public int DeleteItem(int id)
        {
            return database.Delete<ProfileModel>(id);
        }

        public int SaveItem(ProfileModel item)
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
