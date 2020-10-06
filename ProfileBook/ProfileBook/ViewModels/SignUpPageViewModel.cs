using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ProfileBook.Models;
using ProfileBook.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class SignUpPageViewModel : ViewModelBase
    {
        public ICommand SignUpClickCommand => new Command(SignUpClick);

        private IRepositoryService RepositoryService { get; }
        public SignUpPageViewModel(IRepositoryService repositoryService,
            INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Users SignUp";
            RepositoryService = repositoryService;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            IsButtonEnabled = false;
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

            if (ValidateLogin(UserLogin, ref message) && ValidatePassword(UserPassword, ConfirmUserPassword, ref message))
            {
                int result = RepositoryService.SaveItem(new UserModel { Name = UserLogin, Password = UserPassword });
                if (result != -1)
                {
                    UserDialogs.Instance.Alert($"User was successfully registrated", "Registration is successful", "OK");

                    var parameters = new NavigationParameters();
                    parameters.Add(nameof(UserLogin), UserLogin);

                    await NavigationService.GoBackAsync(parameters);
                }
                else
                {
                    UserDialogs.Instance.Alert($"User with such login already exists", "Registration failed", "OK");
                }
            }
            else if (!string.IsNullOrEmpty(message))
            {
                UserDialogs.Instance.Alert(message, "Password is incorrect", "OK");
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

        private bool ValidatePassword(string pass, string confpass, ref string message)
        {
            bool result = true;

            if (pass != confpass)
            {
                UserDialogs.Instance.Alert("Passwords must match!", "Passwords don't match", "OK");

                result = false;
                return result;
            }

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
            if (string.IsNullOrEmpty(userLogin) || string.IsNullOrEmpty(userPassword) || string.IsNullOrEmpty(ConfirmUserPassword))
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
