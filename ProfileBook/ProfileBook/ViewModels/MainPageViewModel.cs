using Prism.Navigation;
using ProfileBook.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ICommand LogOutClickCommand => new Command(LogOutClick);
        public ICommand SettingsClickCommand => new Command(SettingsClick);
        public ICommand EditClickCommand => new Command<ProfileModel>(EditClick);
        public ICommand DeleteClickCommand => new Command(DeleteClick);
        public ICommand AddButtonClickCommand => new Command(AddButtonClick);

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            ProfileList = new ObservableCollection<ProfileModel>
            {
                new ProfileModel()
                {
                    ProfileImage="https://docs.microsoft.com/ru-ru/xamarin/xamarin-forms/user-interface/images-images/local-sml.png",
                    NickNameLabel="dima",
                    NameLabel="Dima Fedchenko",
                    DateLabel=DateTime.Now
                },
                new ProfileModel()
                {
                    ProfileImage="https://docs.microsoft.com/ru-ru/xamarin/xamarin-forms/user-interface/images-images/local-sml.png",
                    NickNameLabel="dima",
                    NameLabel="Dima Fedchenko",
                    DateLabel=DateTime.Now
                },new ProfileModel()
                {
                    ProfileImage="https://docs.microsoft.com/ru-ru/xamarin/xamarin-forms/user-interface/images-images/local-sml.png",
                    NickNameLabel="dima",
                    NameLabel="Dima Fedchenko",
                    DateLabel=DateTime.Now
                },new ProfileModel()
                {
                    ProfileImage="https://docs.microsoft.com/ru-ru/xamarin/xamarin-forms/user-interface/images-images/local-sml.png",
                    NickNameLabel="dima",
                    NameLabel="Dima Fedchenko",
                    DateLabel=DateTime.Now
                },new ProfileModel()
                {
                    ProfileImage="https://docs.microsoft.com/ru-ru/xamarin/xamarin-forms/user-interface/images-images/local-sml.png",
                    NickNameLabel="dima",
                    NameLabel="Dima Fedchenko",
                    DateLabel=DateTime.Now
                },new ProfileModel()
                {
                    ProfileImage="https://docs.microsoft.com/ru-ru/xamarin/xamarin-forms/user-interface/images-images/local-sml.png",
                    NickNameLabel="dima",
                    NameLabel="Dima Fedchenko",
                    DateLabel=DateTime.Now
                },new ProfileModel()
                {
                    ProfileImage="https://docs.microsoft.com/ru-ru/xamarin/xamarin-forms/user-interface/images-images/local-sml.png",
                    NickNameLabel="dima",
                    NameLabel="Dima Fedchenko",
                    DateLabel=DateTime.Now
                },new ProfileModel()
                {
                    ProfileImage="https://docs.microsoft.com/ru-ru/xamarin/xamarin-forms/user-interface/images-images/local-sml.png",
                    NickNameLabel="dima",
                    NameLabel="Dima Fedchenko",
                    DateLabel=DateTime.Now
                },new ProfileModel()
                {
                    ProfileImage="https://docs.microsoft.com/ru-ru/xamarin/xamarin-forms/user-interface/images-images/local-sml.png",
                    NickNameLabel="dima",
                    NameLabel="Dima Fedchenko",
                    DateLabel=DateTime.Now
                },new ProfileModel()
                {
                    ProfileImage="https://docs.microsoft.com/ru-ru/xamarin/xamarin-forms/user-interface/images-images/local-sml.png",
                    NickNameLabel="dima",
                    NameLabel="Dima Fedchenko",
                    DateLabel=DateTime.Now
                },new ProfileModel()
                {
                    ProfileImage="https://docs.microsoft.com/ru-ru/xamarin/xamarin-forms/user-interface/images-images/local-sml.png",
                    NickNameLabel="dima",
                    NameLabel="Dima Fedchenko",
                    DateLabel=DateTime.Now
                },
            };
        }

        private ProfileModel selectedProfile;
        public ProfileModel SelectedProfile
        {
            get { return selectedProfile; }
            set
            {
                SetProperty(ref selectedProfile, value);
            }
        }

        private ObservableCollection<ProfileModel> profileList;
        public ObservableCollection<ProfileModel> ProfileList
        {
            get { return profileList; }
            set { SetProperty(ref profileList, value); }
        }

        private async void LogOutClick()
        {
            await NavigationService.NavigateAsync("/NavigationPage/SignInPage");
        }

        private async void SettingsClick()
        {
            await NavigationService.NavigateAsync("SettingsPage");
        }

        private async void EditClick(ProfileModel model)
        {
            var parametеrs = new NavigationParameters();
            parametеrs.Add(nameof(model.DateLabel), model.DateLabel);

            await NavigationService.NavigateAsync("AddEditProfilePage", parametеrs);
        }

        private async void AddButtonClick()
        {
            await NavigationService.NavigateAsync("AddEditProfilePage");
        }

        private void DeleteClick()
        {
        }
    }
}
