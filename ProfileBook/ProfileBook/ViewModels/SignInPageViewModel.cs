using Prism.Navigation;
using Prism.Services;
using ProfileBook.Helpers;
using ProfileBook.Models;
using ProfileBook.Resources;
using ProfileBook.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class SignInPageViewModel : ViewModelBase
    {
        public ICommand LabelClickCommand => new Command(LabelClick);
        public ICommand SignInClickCommand => new Command(SignInClick);

        private IRepositoryService RepositoryService { get; }
        private IPageDialogService PageDialogService { get; }
        public SignInPageViewModel(INavigationService navigationService,
            IRepositoryService repositoryService,
            IPageDialogService pageDialogService)
            : base(navigationService)
        {
            RepositoryService = repositoryService;
            PageDialogService = pageDialogService;
            RepositoryService.InitTable<UserModel>();

            if (!string.IsNullOrEmpty(Settings.RememberedLogin))
            {
                UserLogin = Settings.RememberedLogin;
            }
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
            var myquery = await RepositoryService.GetAsync<UserModel>((u => u.Name.Equals(UserLogin) && u.Password.Equals(UserPassword)));

            if (myquery != null)
            {
                Settings.RememberedLogin = UserLogin;
                Settings.RememberedUserId = myquery.Id;

                await NavigationService.NavigateAsync("/NavigationPage/MainListPage");
            }
            else
            {
                await PageDialogService.DisplayAlertAsync(AppResources.InvalidLogin, AppResources.InvalidLogin, "OK");
                Settings.RememberedLogin = string.Empty;
                Settings.RememberedUserId = 0;
                UserPassword = string.Empty;
            }
        }

        private void SwitchButtonEnabled()
        {
            IsButtonEnabled = !string.IsNullOrEmpty(UserLogin) && !string.IsNullOrEmpty(UserPassword);
        }
    }
}
