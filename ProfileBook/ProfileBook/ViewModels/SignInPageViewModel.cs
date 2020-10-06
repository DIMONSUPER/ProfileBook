using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ProfileBook.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class SignInPageViewModel : ViewModelBase
    {
        public ICommand LabelClickCommand => new Command(LabelClick);

        public ICommand SignInClickCommand => new Command(SignInClick);

        private IRepositoryService RepositoryService { get; }
        public SignInPageViewModel(IRepositoryService repositoryService,
            INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Users SignIn";
            RepositoryService = repositoryService;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            IsButtonEnabled = false;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(nameof(UserLogin), out string userLogin))
            {
                UserLogin = userLogin;
            }
        }

        private bool isButtonEnabled;
        public bool IsButtonEnabled
        {
            get { return isButtonEnabled; }
            set { SetProperty(ref isButtonEnabled, value); }
        }

        private string userLogin;
        public string UserLogin
        {
            get { return userLogin; }
            set
            {
                SetProperty(ref userLogin, value);

                SwitchButtonEnabled();
            }
        }

        private string userPassword;
        public string UserPassword
        {
            get { return userPassword; }
            set
            {
                SetProperty(ref userPassword, value);

                SwitchButtonEnabled();
            }
        }

        private async void LabelClick()
        {
            await NavigationService.NavigateAsync("SignUpPage");
        }

        private async void SignInClick()
        {
            var myquery = RepositoryService.GetItems().Where(u => u.Name.Equals(UserLogin) && u.Password.Equals(UserPassword)).FirstOrDefault();

            if (myquery != null)
            {
                await NavigationService.NavigateAsync("MainPage");
            }
            else
            {
                UserDialogs.Instance.Alert("Invalid login or password!", "Error", "OK");
                UserPassword = string.Empty;
            }
        }

        private void SwitchButtonEnabled()
        {
            if (string.IsNullOrEmpty(userLogin) || string.IsNullOrEmpty(userPassword))
            {
                IsButtonEnabled = false;
            }
            else
            {
                IsButtonEnabled = true;
            }
        }
    }
}
