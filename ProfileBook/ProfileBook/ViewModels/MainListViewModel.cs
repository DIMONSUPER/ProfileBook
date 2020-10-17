using Acr.UserDialogs;
using Prism.Navigation;
using ProfileBook.Helpers;
using ProfileBook.Models;
using ProfileBook.Resources;
using ProfileBook.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        private IRepositoryService RepositoryService { get; }
        private IUserDialogs UserDialogs { get; }
        public MainListViewModel(INavigationService navigationService,
            IRepositoryService repositoryService,
            IUserDialogs userDialogs)
            : base(navigationService)
        {
            RepositoryService = repositoryService;
            UserDialogs = userDialogs;
            RepositoryService.InitTable<ProfileModel>();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            RefreshList();
        }

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
            parametеrs.Add(nameof(model.ProfileImage), model.ProfileImage);
            parametеrs.Add(nameof(model.NickNameLabel), model.NickNameLabel);
            parametеrs.Add(nameof(model.NameLabel), model.NameLabel);
            parametеrs.Add(nameof(model.Description), model.Description);
            parametеrs.Add(nameof(model.Id), model.Id);
            parametеrs.Add(nameof(model.DateLabel), model.DateLabel);

            await NavigationService.NavigateAsync("AddEditProfilePage", parametеrs);
        }

        private async void AddButtonClick()
        {
            await NavigationService.NavigateAsync("AddEditProfilePage");
        }

        public async void RefreshList()
        {
            var profiles = await RepositoryService.GetAllAsync<ProfileModel>(p => p.UserId == Settings.RememberedUserId);
            if (Settings.RememberedRadioButton == "SortByDate" || string.IsNullOrEmpty(Settings.RememberedRadioButton))
            {
                profiles = profiles.OrderBy(p => p.DateLabel);
            }
            else if (Settings.RememberedRadioButton == "SortByName")
            {
                profiles = profiles.OrderBy(p => p.NameLabel);
            }
            else if (Settings.RememberedRadioButton == "SortByNickName")
            {
                profiles = profiles.OrderBy(p => p.NickNameLabel);
            }

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
                await RepositoryService.DeleteAsync(model);
                RefreshList();
            }
        }
    }
}
