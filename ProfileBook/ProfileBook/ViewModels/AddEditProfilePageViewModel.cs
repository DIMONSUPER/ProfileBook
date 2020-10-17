using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using ProfileBook.Helpers;
using ProfileBook.Models;
using ProfileBook.Resources;
using ProfileBook.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class AddEditProfilePageViewModel : ViewModelBase
    {
        public ICommand SaveClickCommand => new Command(SaveClick);
        public ICommand ImageClickCommand => new Command(ImageClick);

        private IRepositoryService RepositoryService { get; }
        private IUserDialogs UserDialogs { get; }
        public AddEditProfilePageViewModel(INavigationService navigationService,
            IRepositoryService repositoryService,
            IUserDialogs userDialogs)
            : base(navigationService)
        {
            RepositoryService = repositoryService;
            UserDialogs = userDialogs;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(nameof(DateLabel), out DateTime dateLabel)
                && parameters.TryGetValue(nameof(ProfileImage), out string profileImage)
                && parameters.TryGetValue("NickNameLabel", out string nickNameLabel)
                && parameters.TryGetValue("NameLabel", out string nameLabel)
                && parameters.TryGetValue(nameof(Description), out string description)
                && parameters.TryGetValue("Id", out int id))
            {
                ProfileImage = profileImage;
                NickName = nickNameLabel;
                Name = nameLabel;
                Description = description;
                ProfileId = id;
                DateLabel = dateLabel;
            }
            else
            {
                ProfileImage = "pic_profile.png";
            }
        }

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
            RepositoryService.InitTable<ProfileModel>();
            if (IsButtonEnabled())
            {
                if (DateLabel == new DateTime())
                {
                    DateLabel = DateTime.Now;
                }

                int result = await RepositoryService.InsertAsync(new ProfileModel
                {
                    Id = ProfileId,
                    NameLabel = Name,
                    DateLabel = DateLabel,
                    NickNameLabel = NickName,
                    ProfileImage = profileImage,
                    Description = Description,
                    UserId = Settings.RememberedUserId
                });

                if (result != -1)
                {
                    var parameters = new NavigationParameters();
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
