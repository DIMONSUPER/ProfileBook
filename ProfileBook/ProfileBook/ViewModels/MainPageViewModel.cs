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
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";
        }

        private ObservableCollection<ProfileModel> profileList;
        public ObservableCollection<ProfileModel> ProfileList
        {
            get { return profileList; }
            set { SetProperty(ref profileList, value); }
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

        private async void LogOutClick()
        {
            await NavigationService.NavigateAsync("/NavigationPage/SignInPage");
        }
    }
}
