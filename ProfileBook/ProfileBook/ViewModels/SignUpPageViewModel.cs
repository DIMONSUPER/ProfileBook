using Prism.Navigation;
using Prism.Services;
using ProfileBook.Models;
using ProfileBook.Services.UserRepository;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class SignUpPageViewModel : ViewModelBase
    {
        public ICommand SignUpClickCommand => new Command(SignUpClick);

        private IUserRepositoryService UserRepositoryService { get; }
        private IPageDialogService PageDialogService { get; }
        public SignUpPageViewModel(INavigationService navigationService,
            IUserRepositoryService userRepositoryService,
            IPageDialogService pageDialogService)
            : base(navigationService)
        {
            Title = "Users SignUp";
            UserRepositoryService = userRepositoryService;
            PageDialogService = pageDialogService;
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

        private string confirmUserPassword;
        public string ConfirmUserPassword
        {
            get { return confirmUserPassword; }
            set
            {
                SetProperty(ref confirmUserPassword, value);
                SwitchButtonEnabled();
            }
        }

        private async void SignUpClick()
        {
            string message = string.Empty;

            if (ValidateLogin(UserLogin, ref message))
            {
                if (UserPassword != ConfirmUserPassword)
                {
                    await PageDialogService.DisplayAlertAsync("Passwords don't match", "Passwords must match!", "OK");
                }
                else if (ValidatePassword(UserPassword, ref message))
                {
                    int result = UserRepositoryService.SaveItem(new UserModel { Name = UserLogin, Password = UserPassword });
                    if (result != -1)
                    {
                        await PageDialogService.DisplayAlertAsync("Registration is successful", "User was successfully registrated", "OK");

                        var parameters = new NavigationParameters();
                        parameters.Add(nameof(UserLogin), UserLogin);

                        await NavigationService.GoBackAsync(parameters);
                    }
                    else
                    {
                        await PageDialogService.DisplayAlertAsync("Registration failed", "User with such login already exists", "OK");
                    }
                }
            }

            if (!string.IsNullOrEmpty(message))
            {
                await PageDialogService.DisplayAlertAsync("Password is incorrect", message, "OK");
            }
        }

        private bool ValidateLogin(string login, ref string message)
        {
            bool result = true;

            var hasMinChars = new Regex(@"^.{4,}");
            if (!hasMinChars.IsMatch(login))
            {
                message += "\nLogin should contain at least 4 characters";

                result = false;
            }

            var hasMaxChars = new Regex(@"^.{1,16}$");
            if (!hasMaxChars.IsMatch(login))
            {
                message += "\nLogin should contain at max 16 characters";

                result = false;
            }

            return result;
        }

        private bool ValidatePassword(string pass, ref string message)
        {
            bool result = true;

            var hasBeginNotNumber = new Regex(@"^\D");
            if (!hasBeginNotNumber.IsMatch(pass))
            {
                message += "Password shouldn't start from a number";

                result = false;
            }

            var hasNumber = new Regex(@"[0-9]+");
            if (!hasNumber.IsMatch(pass))
            {
                message += "\nPassword should contain number";

                result = false;
            }

            var hasLowerChar = new Regex(@"[a-z]+");
            if (!hasLowerChar.IsMatch(pass))
            {
                message += "\nPassword should contain lower case character";

                result = false;
            }

            var hasUpperChar = new Regex(@"[A-Z]+");
            if (!hasUpperChar.IsMatch(pass))
            {
                message += "\nPassword should contain upper case character";

                result = false;
            }

            var hasMinChars = new Regex(@"^.{8,}");
            if (!hasMinChars.IsMatch(pass))
            {
                message += "\nPassword should contain at least 8 characters";

                result = false;
            }

            var hasMaxChars = new Regex(@"^.{1,16}$");
            if (!hasMaxChars.IsMatch(pass))
            {
                message += "\nPassword should contain at max 16 characters";

                result = false;
            }

            return result;
        }

        private void SwitchButtonEnabled()
        {
            IsButtonEnabled = !string.IsNullOrEmpty(UserLogin) && !string.IsNullOrEmpty(UserPassword) && !string.IsNullOrEmpty(ConfirmUserPassword);
        }
    }
}
