using Acr.UserDialogs;
using Prism.Navigation;
using ProfileBook.Helpers;
using ProfileBook.Models;
using ProfileBook.Resources;
using ProfileBook.Services.ProfileRepository;
using ProfileBook.Services.UserRepository;
using ProfileBook.Themes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class MainListViewModel : ViewModelBase
    {
        public ICommand LogOutClickCommand => new Command(LogOutClick);
        public ICommand SettingsClickCommand => new Command(SettingsClick);
        public ICommand EditClickCommand => new Command<ProfileModel>(EditClick);
        public ICommand DeleteClickCommand => new Command<ProfileModel>(DeleteClick);
        public ICommand AddButtonClickCommand => new Command(AddButtonClick);
        public ICommand ProfileClickCommand => new Command<ProfileModel>(ProfileClick);

        private IProfileRepositoryService ProfileRepositoryService { get; }
        private IUserRepositoryService UserRepositoryService { get; }
        private IUserDialogs UserDialogs { get; }
        public MainListViewModel(INavigationService navigationService,
            IUserRepositoryService userRepositoryService,
            IProfileRepositoryService profileRepositoryService,
            IUserDialogs userDialogs)
            : base(navigationService)
        {
            ProfileRepositoryService = profileRepositoryService;
            UserRepositoryService = userRepositoryService;
            UserDialogs = userDialogs;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("Id", out int userId))
            {
                UserId = userId;
            }
            else if (!string.IsNullOrEmpty(Settings.RememberedLogin))
            {
                var user = UserRepositoryService.GetItems().FirstOrDefault(u => u.Name == Settings.RememberedLogin);
                UserId = user.Id;
            }
            RefreshList();
        }

        public int UserId { get; set; }

        private ProfileModel selectedProfile;
        public ProfileModel SelectedProfile
        {
            get { return selectedProfile; }
            set
            {
                SetProperty(ref selectedProfile, value);
                ProfileClickCommand.Execute(selectedProfile);
            }
        }

        private bool isListVisible;
        public bool IsListVisible
        {
            get { return isListVisible; }
            set { SetProperty(ref isListVisible, value); }
        }

        private bool isLabelVisible;
        public bool IsLabelVisible
        {
            get { return isLabelVisible; }
            set { SetProperty(ref isLabelVisible, value); }
        }

        private ObservableCollection<ProfileModel> profileList;
        public ObservableCollection<ProfileModel> ProfileList
        {
            get { return profileList; }
            set { SetProperty(ref profileList, value); }
        }

        private async void LogOutClick()
        {
            Settings.RememberedLogin = string.Empty;
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
            var parameters = new NavigationParameters();
            parameters.Add(nameof(UserId), UserId);

            await NavigationService.NavigateAsync("AddEditProfilePage", parameters);
        }

        public void RefreshList()
        {
            var profiles = ProfileRepositoryService.GetItems().Where(p => p.UserId == UserId);

            if (Settings.RememberedRadioButton == "SortByDate" || string.IsNullOrEmpty(Settings.RememberedRadioButton))
                profiles = profiles.OrderBy(p => p.DateLabel);
            else if (Settings.RememberedRadioButton == "SortByName")
                profiles = profiles.OrderBy(p => p.NameLabel);
            else if (Settings.RememberedRadioButton == "SortByNickName")
                profiles = profiles.OrderBy(p => p.NickNameLabel);

            if (profiles.ToList().Count != 0)
            {
                ProfileList = new ObservableCollection<ProfileModel>(profiles);
                IsListVisible = true;
                IsLabelVisible = false;
            }
            else
            {
                IsListVisible = false;
                IsLabelVisible = true;
            }
        }

        private async void ProfileClick(ProfileModel model)
        {
            var parameters = new NavigationParameters();
            parameters.Add(nameof(model.ProfileImage), model.ProfileImage);

            await NavigationService.NavigateAsync("ImagePopupPage", parameters);
        }

        private async void DeleteClick(ProfileModel model)
        {
            var result = await UserDialogs.ConfirmAsync(new ConfirmConfig()
                .SetTitle(AppResources.ConfirmationTitle)
                .SetOkText(AppResources.Yes)
                .SetCancelText(AppResources.No));

            if (result)
            {
                ProfileRepositoryService.DeleteItem(model.Id);
                RefreshList();
            }
        }
    }
}
