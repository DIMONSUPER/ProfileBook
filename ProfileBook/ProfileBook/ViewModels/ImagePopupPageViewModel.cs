using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class ImagePopupPageViewModel : ViewModelBase
    {
        public ICommand ImageClickCommand => new Command(ImageClick);

        public ImagePopupPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(nameof(ProfileImage), out string profileImage))
            {
                ProfileImage = profileImage;
            }
            else
            {
                ProfileImage = "pic_profile.png";
            }
        }

        private string profileImage;
        public string ProfileImage
        {
            get { return profileImage; }
            set { SetProperty(ref profileImage, value); }
        }

        private async void ImageClick()
        {
            await NavigationService.GoBackAsync();
        }
    }
}
