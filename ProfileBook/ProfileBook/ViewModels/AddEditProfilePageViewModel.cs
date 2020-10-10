using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using ProfileBook.Models;
using ProfileBook.Services.ProfileRepository;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class AddEditProfilePageViewModel : ViewModelBase
    {
        public Command SaveClickCommand => new Command(SaveClick, SwitchButtonEnabled);
        public ICommand ImageClickCommand => new Command(ImageClick);

        private IProfileRepositoryService ProfileRepositoryService { get; }
        public AddEditProfilePageViewModel(INavigationService navigationService,
            IProfileRepositoryService profileRepositoryService)
            : base(navigationService)
        {
            Title = "Add Profile";
            ProfileRepositoryService = profileRepositoryService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(nameof(UserId), out int userId))
            {
                UserId = userId;
                ProfileImage = "pic_profile.png";
            }
            else if (parameters.TryGetValue("DateLabel", out DateTime dateLabel))
            {
                var myprofile = ProfileRepositoryService.GetItems().FirstOrDefault(p => p.DateLabel.Equals(dateLabel));

                ProfileImage = myprofile.ProfileImage;
                NickName = myprofile.NickNameLabel;
                Name = myprofile.NameLabel;
                Description = myprofile.Description;
                UserId = myprofile.UserId;
                ProfileId = myprofile.Id;
                DateLabel = myprofile.DateLabel;
            }
        }

        public int UserId { get; set; }
        public int ProfileId { get; set; }
        public DateTime DateLabel { get; set; }

        /*private bool isButtonEnabled;
        public bool IsButtonEnabled
        {
            get { return isButtonEnabled; }
            set
            {
                SetProperty(ref isButtonEnabled, value);
            }
        }*/

        private string profileImage;
        public string ProfileImage
        {
            get { return profileImage; }
            set { SetProperty(ref profileImage, value); }
        }

        private string nickName;
        public string NickName
        {
            get { return nickName; }
            set
            {
                SetProperty(ref nickName, value);

                //SwitchButtonEnabled();
                SaveClickCommand.ChangeCanExecute();
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(ref name, value);

                //SwitchButtonEnabled();
                SaveClickCommand.ChangeCanExecute();
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        private void ImageClick()
        {
            UserDialogs.Instance.ActionSheet(new ActionSheetConfig()
                                             .SetTitle("Choose picture from")
                                             .Add("Camera", ChooseFromCamera, "ic_camera_alt.png")
                                             .Add("Gallery", ChooseFromGallery, "ic_collections.png"));
        }

        private async void ChooseFromCamera()
        {
            try
            {
                if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
                {
                    MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        SaveToAlbum = true,
                        Directory = "Sample",
                        Name = $"{DateTime.Now:dd.MM.yyyy_hh.mm.ss}.jpg"
                    });

                    if (file == null)
                        return;

                    ProfileImage = file.Path;
                }
            }
            catch
            { }
        }

        private async void ChooseFromGallery()
        {
            try
            {
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    MediaFile photo = await CrossMedia.Current.PickPhotoAsync();
                    ProfileImage = photo.Path;
                }
            }
            catch
            { }
        }

        private async void SaveClick()
        {

            if (DateLabel == new DateTime())
                DateLabel = DateTime.Now;

            int result = ProfileRepositoryService.SaveItem(new ProfileModel
            {
                Id = ProfileId,
                NameLabel = Name,
                DateLabel = DateLabel,
                NickNameLabel = NickName,
                ProfileImage = profileImage,
                Description = Description,
                UserId = UserId
            });

            if (result != -1)
            {
                var parameters = new NavigationParameters();
                parameters.Add("Id", UserId);
                await NavigationService.GoBackAsync(parameters);
            }

        }

        private bool SwitchButtonEnabled()
        {
            bool result = !string.IsNullOrEmpty(NickName) && !string.IsNullOrEmpty(Name);
            return result;
        }
    }
}
