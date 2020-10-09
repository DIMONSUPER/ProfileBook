using Prism.Navigation;
using Prism.Services;
using ProfileBook.Services;
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
        private IPageDialogService PageDialogService { get; }
        public SignInPageViewModel(INavigationService navigationService,
            IRepositoryService repositoryService,
            IPageDialogService pageDialogService)
            : base(navigationService)
        {
            Title = "Users SignIn";
            RepositoryService = repositoryService;
            PageDialogService = pageDialogService;
            IsButtonEnabled = false;
            UserLogin = "dima";
            UserPassword = "Q1W2E3r4t5y6";
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
            var myquery = RepositoryService.GetItems().FirstOrDefault(u => u.Name.Equals(UserLogin) && u.Password.Equals(UserPassword));

            if (myquery != null)
            {
                await NavigationService.NavigateAsync("/NavigationPage/MainPage");
            }
            else
            {
                await PageDialogService.DisplayAlertAsync("Invalid login or password!", "Invalid login or password!", "OK");
                UserPassword = string.Empty;
            }
        }

        private void SwitchButtonEnabled()
        {
            IsButtonEnabled = !string.IsNullOrEmpty(UserLogin) && !string.IsNullOrEmpty(UserPassword);
        }
    }
}
