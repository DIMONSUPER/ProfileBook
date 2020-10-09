using ProfileBook.Models;
using System.Collections.Generic;

namespace ProfileBook.Services.ProfileRepository
{
    public interface IProfileRepositoryService
    {
        IEnumerable<ProfileModel> GetItems();
        ProfileModel GetItem(int id);
        int DeleteItem(int id);
        int SaveItem(ProfileModel item);
    }
}
