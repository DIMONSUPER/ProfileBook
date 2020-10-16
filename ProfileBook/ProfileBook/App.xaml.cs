using Prism;
using Prism.Ioc;
using ProfileBook.ViewModels;
using ProfileBook.Views;
using Xamarin.Forms;
using ProfileBook.Services.ProfileRepository;
using ProfileBook.Services.UserRepository;
using ProfileBook.Helpers;
using Prism.Plugin.Popups;
using Acr.UserDialogs;
using Plugin.Settings.Abstractions;
using Plugin.Settings;
using System.Collections.Generic;
using ProfileBook.Themes;
using System.Globalization;
using System.Linq;
using System.Threading;
using ProfileBook.Resources;

namespace ProfileBook
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }
        protected override async void OnInitialized()
        {
            InitializeComponent();

            XF.Material.Forms.Material.Init(this);

            if (string.IsNullOrEmpty(Settings.RememberedLanguage))
            {
                string shortLanguage = CultureInfo.InstalledUICulture.DisplayName.Split(' ')[0];
                if (shortLanguage == "English" || shortLanguage == "Russian" || shortLanguage == "Ukrainian")
                {
                    Settings.RememberedLanguage = shortLanguage;
                }
                else
                {
                    Settings.RememberedLanguage = "English";
                }
            }
            var language = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList()
                   .First(element => element.EnglishName.Contains(Settings.RememberedLanguage.ToString()));
            Thread.CurrentThread.CurrentUICulture = language;
            AppResources.Culture = language;

            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                if (Settings.RememberedCheckBox == false)
                {
                    mergedDictionaries.Add(new LightTheme());
                }
                else
                {
                    mergedDictionaries.Add(new DarkTheme());
                }
            }

            if (!string.IsNullOrEmpty(Settings.RememberedLogin))
            {
                await NavigationService.NavigateAsync("NavigationPage/MainListPage");
            }
            else
            {
                await NavigationService.NavigateAsync("NavigationPage/SignInPage");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IUserRepositoryService>(Container.Resolve<UserRepositoryService>());
            containerRegistry.RegisterInstance<IProfileRepositoryService>(Container.Resolve<ProfileRepositoryService>());

            containerRegistry.RegisterInstance<IUserDialogs>(UserDialogs.Instance);
            containerRegistry.RegisterInstance<ISettings>(CrossSettings.Current);

            containerRegistry.RegisterPopupNavigationService();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainListPage, MainListViewModel>();
            containerRegistry.RegisterForNavigation<SignInPage, SignInPageViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpPageViewModel>();
            containerRegistry.RegisterForNavigation<AddEditProfilePage, AddEditProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<ImagePopupPage, ImagePopupPageViewModel>();
        }
    }
}
