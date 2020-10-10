using Prism;
using Prism.Ioc;
using ProfileBook.ViewModels;
using ProfileBook.Views;
using Xamarin.Forms;
using ProfileBook.Services.ProfileRepository;
using ProfileBook.Services.UserRepository;
using ProfileBook.Helpers;
using Prism.Plugin.Popups;

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
