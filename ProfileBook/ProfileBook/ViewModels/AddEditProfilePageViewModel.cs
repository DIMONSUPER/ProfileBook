using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ProfileBook.Models;
using ProfileBook.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ProfileBook.ViewModels
{
    public class AddEditProfilePageViewModel : ViewModelBase
    {
        public ICommand SaveClickCommand => new Command(SaveClick);

        private IRepositoryService RepositoryService { get; }
        public AddEditProfilePageViewModel(INavigationService navigationService,
            IRepositoryService repositoryService)
            : base(navigationService)
        {
            Title = "Add Profile";
            RepositoryService = repositoryService;
            IsButtonEnabled = false;
            ProfileImage = "pic_profile.png";
        }

        private bool isButtonEnabled;
        public bool IsButtonEnabled
        {
            get { return isButtonEnabled; }
            set { SetProperty(ref isButtonEnabled, value); }
        }

        private string profileImage;
        public string ProfileImage
        {
            get { return profileImage; }
            set { SetProperty(ref profileImage, value); }
        }

        private string nickName;
        public string NickName
        {
            get { return nickName; }
            set
            {
                SetProperty(ref nickName, value);
                SwitchButtonEnabled();
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(ref name, value);
                SwitchButtonEnabled();
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        private async void SaveClick()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var database = new SQLiteConnection(Path.Combine(path, "Profiles.db"));
            database.CreateTable<ProfileModel>();

            var myquery = database.Insert(new ProfileModel
            {
                DateLabel = DateTime.Now,
                NameLabel = Name,
                NickNameLabel = NickName,
                ProfileImage = ProfileImage
            });

            await NavigationService.GoBackAsync();

        }

        private void SwitchButtonEnabled()
        {
            IsButtonEnabled = !string.IsNullOrEmpty(NickName) && !string.IsNullOrEmpty(Name);
        }
    }
}
