using Prism.Navigation;
using Prism.Services;
using ProfileBook.Helpers;
using ProfileBook.Resources;
using ProfileBook.Services.UserRepository;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class SignInPageViewModel : ViewModelBase
    {
        public ICommand LabelClickCommand => new Command(LabelClick);
        public ICommand SignInClickCommand => new Command(SignInClick);

        private IUserRepositoryService UserRepositoryService { get; }
        private IPageDialogService PageDialogService { get; }
        public SignInPageViewModel(INavigationService navigationService,
            IUserRepositoryService userRepositoryService,
            IPageDialogService pageDialogService)
            : base(navigationService)
        {
            UserRepositoryService = userRepositoryService;
            PageDialogService = pageDialogService;

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
            var myquery = UserRepositoryService.GetItems().FirstOrDefault(u => u.Name.Equals(UserLogin) && u.Password.Equals(UserPassword));

            if (myquery != null)
            {
                Settings.RememberedLogin = UserLogin;

                var parameters = new NavigationParameters();
                parameters.Add(nameof(myquery.Id), myquery.Id);

                await NavigationService.NavigateAsync("/NavigationPage/MainListPage", parameters);
            }
            else
            {
                await PageDialogService.DisplayAlertAsync(AppResources.InvalidLogin, AppResources.InvalidLogin, "OK");
                Settings.RememberedLogin = string.Empty;
                UserPassword = string.Empty;
            }
        }

        private void SwitchButtonEnabled()
        {
            IsButtonEnabled = !string.IsNullOrEmpty(UserLogin) && !string.IsNullOrEmpty(UserPassword);
        }
    }
}
