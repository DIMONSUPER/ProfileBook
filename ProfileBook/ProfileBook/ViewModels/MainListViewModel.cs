using Acr.UserDialogs;
using Prism.Navigation;
using ProfileBook.Helpers;
using ProfileBook.Models;
using ProfileBook.Services.ProfileRepository;
using ProfileBook.Services.UserRepository;
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

        IProfileRepositoryService ProfileRepositoryService { get; }
        IUserRepositoryService UserRepositoryService { get; }
        public MainListViewModel(INavigationService navigationService,
            IUserRepositoryService userRepositoryService,
            IProfileRepositoryService profileRepositoryService)
            : base(navigationService)
        {
            Title = "Main Page";
            ProfileRepositoryService = profileRepositoryService;
            UserRepositoryService = userRepositoryService;
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
            set{ SetProperty(ref profileList, value); }
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
            if (profiles.ToList().Count!=0)
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

        private async void DeleteClick(ProfileModel model)
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig()
                .SetTitle("Вы действительно хотите удалить?")
                .SetOkText("Да")
                .SetCancelText("Нет"));

            if (result)
            {
                ProfileRepositoryService.DeleteItem(model.Id);
                RefreshList();
            }
        }
    }
}
