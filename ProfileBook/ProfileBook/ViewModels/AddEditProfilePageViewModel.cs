using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using ProfileBook.Models;
using ProfileBook.Resources;
using ProfileBook.Services.ProfileRepository;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class AddEditProfilePageViewModel : ViewModelBase
    {
        public ICommand SaveClickCommand => new Command(SaveClick);
        public ICommand ImageClickCommand => new Command(ImageClick);

        private IProfileRepositoryService ProfileRepositoryService { get; }
        private IUserDialogs UserDialogs { get; }
        public AddEditProfilePageViewModel(INavigationService navigationService,
            IProfileRepositoryService profileRepositoryService,
            IUserDialogs userDialogs)
            : base(navigationService)
        {
            ProfileRepositoryService = profileRepositoryService;
            UserDialogs = userDialogs;
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
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(ref name, value);
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
            UserDialogs.ActionSheet(new ActionSheetConfig()
                                             .SetTitle(AppResources.ChoosePicture)
                                             .Add(AppResources.Camera, ChooseFromCamera, "ic_camera_alt.png")
                                             .Add(AppResources.Gallery, ChooseFromGallery, "ic_collections.png"));
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
            if (IsButtonEnabled())
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
            else
            {
                UserDialogs.Alert(AppResources.FieldsFilled);
            }
        }

        public bool IsButtonEnabled()
        {
            return !string.IsNullOrEmpty(NickName) && !string.IsNullOrEmpty(Name);
        }
    }
}
